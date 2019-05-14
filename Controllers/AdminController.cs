using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SportsCentre.API.Data;
using SportsCentre.API.Dtos;
using SportsCentre.API.Models;

namespace SportsCentre.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository repo;
        private readonly IConfiguration config;

        public AdminController(IAdminRepository repo, IConfiguration config)
        {
            this.repo = repo;
            this.config = config;
        }


        [HttpPost("createclass")]
        public async Task<IActionResult> CreateNewClass(CreateClassDto createClassDto)
        {
            Class newClass = new Class
            {
                ClassName = createClassDto.ClassName,
                ClassDate = createClassDto.ClassDate,
                Attendant = createClassDto.Attendant,
                Cost = createClassDto.Cost
            };

            Class createdClass = await repo.CreateNewClass(newClass);

            return StatusCode(201);
        }

        
        [HttpPost("editclass")]
        public Task<IActionResult> EditClass(CreateClassDto editClassDto)
        {
            throw new System.Exception();
        }


        [HttpPost("removeclass")]
        public Task<IActionResult> RemoveClass()
        {
            throw new System.Exception();
        }
    } 
}