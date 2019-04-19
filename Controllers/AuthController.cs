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
        private readonly IAuthRepository repo;
        private readonly IConfiguration config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this.config = config;
            this.repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Email = userForRegisterDto.Email.ToLower();

            if (await repo.UserExists(userForRegisterDto.Email))
                return BadRequest("Email Already In Use");

            User userToCreate = new User
            {
                Email = userForRegisterDto.Email
            };

            User createdUser = await repo.Register(userToCreate, userForRegisterDto.Password);

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
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken
            (
                issuer: "localhost",
                audience: "localhost",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token)});
        }
    }
}