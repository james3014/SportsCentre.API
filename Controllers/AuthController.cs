using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SportsCentre.API.Data;
using SportsCentre.API.Dtos;
using SportsCentre.API.Models;

namespace SportsCentre.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Private variables
        private readonly IAuthRepository repo;
        private readonly IConfiguration config;
        private readonly IMapper mapper;
        private readonly IDataRepository dataRepo;




        // Constructor
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper, IDataRepository dataRepo)
        {
            this.config = config;
            this.mapper = mapper;
            this.dataRepo = dataRepo;
            this.repo = repo;
        }


        // Register controller route
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Email = userForRegisterDto.Email.ToLower();

            if (await repo.UserExists(userForRegisterDto.Email)) return BadRequest("Email Already In Use");

            User newUser = new User
            {
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                Surname = userForRegisterDto.Surname,
                AddressLine1 = userForRegisterDto.AddressLine1,
                AddressLine2 = userForRegisterDto.AddressLine2,
                Town = userForRegisterDto.Town,
                PostCode = userForRegisterDto.PostCode,
                DateOfBirth = userForRegisterDto.DateOfBirth
            };

            User createdUser = await repo.Register(newUser, userForRegisterDto.Password);

            return StatusCode(200);
        }

        [HttpPost("staff/create")]
        public async Task<IActionResult> CreateStaff(StaffForRegisterDto staffForRegisterDto)
        {
            staffForRegisterDto.Email = staffForRegisterDto.Email.ToLower();

            if (await repo.UserExists(staffForRegisterDto.Email)) return BadRequest("Email Already In Use");

            Staff staff = new Staff
            {
                Email = staffForRegisterDto.Email,
                FirstName = staffForRegisterDto.FirstName,
                Surname = staffForRegisterDto.Surname,
                AddressLine1 = staffForRegisterDto.AddressLine1,
                AddressLine2 = staffForRegisterDto.AddressLine2,
                Town = staffForRegisterDto.Town,
                PostCode = staffForRegisterDto.PostCode,
                DateOfBirth = staffForRegisterDto.DateOfBirth,
                Role = staffForRegisterDto.Role
            };

            var createdStaff = await repo.CreateStaff(staff, staffForRegisterDto.Password);

            return StatusCode(200);
        }

        [HttpPut("staff/update/{id}")]
        public async Task<IActionResult> UpdateStaff(int id, StaffForRegisterDto staffForRegisterDto)
        {
            staffForRegisterDto.Email = staffForRegisterDto.Email.ToLower();

            if (await repo.UserExists(staffForRegisterDto.Email)) return BadRequest("Email Already In Use");

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


        /**
            This route takes in a JSON "UserForLoginDto" which provides an email and password.
            If this is not null then a new claim is created which will provide our JWT with an
            ID and email address. A key is provided from appsettings.json and this is used as 
            part of the credentials. Finally a token is generated with an expiry date as well
            as the claims and credentials.
         */
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userforLoginDto)
        {
            User userFromRepo = await repo.Login(userforLoginDto.Email, userforLoginDto.Password);

            if (userFromRepo == null) return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, userFromRepo.Email),
                new Claim(ClaimTypes.Name, userFromRepo.FirstName)
            };

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

            var user = mapper.Map<CurrentUserDto>(userFromRepo);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });
        }

        [HttpPost("staff")]
        public async Task<IActionResult> StaffLogin(StaffForLoginDto staffForLoginDto)
        {
            Staff staffFromRepo = await repo.StaffLogin(staffForLoginDto.Email, staffForLoginDto.Password);

            if (staffFromRepo == null) return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, staffFromRepo.Email),
                new Claim(ClaimTypes.Name, staffFromRepo.FirstName),
                new Claim(ClaimTypes.Role, staffFromRepo.Role)
            };

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

            var staff = mapper.Map<CurrentStaffDto>(staffFromRepo);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                staff
            });

        }
    }
}