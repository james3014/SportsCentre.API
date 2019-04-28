using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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


        // Constructor
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this.config = config;
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

            return StatusCode(201);
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

            return Ok(new { token = tokenHandler.WriteToken(token) });
        }

        [HttpPost("staff")]
        public async Task<IActionResult> StaffLogin(UserForLoginDto userforLoginDto)
        {
            Staff staffFromRepo = await repo.StaffLogin(userforLoginDto.Email, userforLoginDto.Password);

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

            return Ok(new { token = tokenHandler.WriteToken(token) });

        }
    }
}