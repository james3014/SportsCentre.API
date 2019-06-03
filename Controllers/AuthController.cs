using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SportsCentre.API.Data;
using SportsCentre.API.Dtos;
using SportsCentre.API.Models;

namespace SportsCentre.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Private variables
        private readonly IAuthRepository repo;
        private readonly IConfiguration config;
        private readonly IMapper mapper;
        private readonly IDataRepository dataRepo;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;



        // Constructor
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper,
            IDataRepository dataRepo, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.config = config;
            this.mapper = mapper;
            this.dataRepo = dataRepo;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.repo = repo;
        }


        // Register controller route
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var userToCreate = mapper.Map<User>(userForRegisterDto);

            var result = await userManager.CreateAsync(userToCreate, userForRegisterDto.Password);

            await userManager.AddToRoleAsync(userToCreate, "User");

            var userToReturn = mapper.Map <UserForDetailedDto>(userToCreate);

            if (result.Succeeded)
            {
                return CreatedAtRoute("GetUser", new { controller = "User", id = userToCreate.Id }, userToReturn);
            }

            return BadRequest(result.Errors);
        }




        [HttpPost("staff/create")]
        public async Task<IActionResult> CreateStaff(StaffForRegisterDto staffForRegisterDto)
        {
            var userToCreate = mapper.Map<User>(staffForRegisterDto);

            var result = await userManager.CreateAsync(userToCreate, staffForRegisterDto.Password);

            await userManager.AddToRoleAsync(userToCreate, "Staff");

            var userToReturn = mapper.Map<UserForDetailedDto>(userToCreate);

            if (result.Succeeded)
            {
                return CreatedAtRoute("GetUser", new { controller = "User", id = userToCreate.Id }, userToReturn);
            }

            return BadRequest(result.Errors);


        }




        [HttpPut("staff/update/{id}")]
        public async Task<IActionResult> UpdateStaff(int id, StaffForRegisterDto staffForRegisterDto)
        {
            staffForRegisterDto.UserName = staffForRegisterDto.UserName.ToLower();

            if (await repo.UserExists(staffForRegisterDto.UserName)) return BadRequest("Username Already In Use");

            var staffForUpdate = await repo.GetStaff(id);

            if (staffForUpdate == null) BadRequest("No Staff Member Found");

            mapper.Map(staffForRegisterDto, staffForUpdate);

            if (await repo.SaveAll())
            {
                return NoContent();
            }

            throw new Exception($"Updating staff {id} failed on save");

        }



        [HttpDelete("staff/delete/{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var staffMember = await repo.GetStaff(id);

            if (staffMember == null) return BadRequest("Staff does not exist");

            await dataRepo.GetStaffClasses(staffMember);

            repo.Delete(staffMember);

            if (await repo.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Failed to delete staff");
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userforLoginDto)
        {
            var user = await userManager.FindByNameAsync(userforLoginDto.UserName);

            var result = await signInManager.CheckPasswordSignInAsync(user, userforLoginDto.Password, false);

            if (result.Succeeded)
            {
                var appUser = await userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName== userforLoginDto.UserName.ToUpper());

                var userToReturn = mapper.Map<CurrentUserDto>(user);

                return Ok(new
                {
                    token = GenerateJwtToken(appUser).Result,
                    user = userToReturn
                });
            }

            return Unauthorized();
        }



        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }



        //[HttpPost("staff")]
        //public async Task<IActionResult> StaffLogin(StaffForLoginDto staffForLoginDto)
        //{
        //    Staff staffFromRepo = await repo.StaffLogin(staffForLoginDto.Email, staffForLoginDto.Password);

        //    if (staffFromRepo == null) return Unauthorized();

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Email, staffFromRepo.Email),
        //        new Claim(ClaimTypes.Name, staffFromRepo.FirstName),
        //        new Claim(ClaimTypes.Role, staffFromRepo.Role)
        //    };

        //    SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));

        //    SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        //    SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(claims),
        //        Expires = DateTime.Now.AddDays(1),
        //        SigningCredentials = creds
        //    };

        //    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        //    SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        //    var staff = mapper.Map<CurrentStaffDto>(staffFromRepo);

        //    return Ok(new
        //    {
        //        token = tokenHandler.WriteToken(token),
        //        staff
        //    });
        //}
    }
}