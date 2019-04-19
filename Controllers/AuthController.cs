using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public AuthController(IAuthRepository repo)
        {
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


    }
}