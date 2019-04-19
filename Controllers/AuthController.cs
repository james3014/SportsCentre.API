using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsCentre.API.Data;
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
        public async Task<IActionResult> Register(string email, string password)
        {
            email = email.ToLower();

            if (await repo.UserExists(email)) 
                return BadRequest("Email Already In Use");

            User userToCreate = new User
            {
                Email = email
            };

            User createdUser = await repo.Register(userToCreate, password);

            return StatusCode(201);
        }


    }
}