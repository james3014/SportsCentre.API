using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SportsCentre.API.Data;
using SportsCentre.API.Dtos;
using SportsCentre.API.Models;
using System.Threading.Tasks;

namespace SportsCentre.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataRepository repo;
        private readonly IConfiguration config;


        public DataController(IDataRepository repo, IConfiguration config)
        {
            this.repo = repo;
            this.config = config;
        }


        [HttpGet("bookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await repo.GetBookings();

            return Ok(bookings);
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

            return Ok(createdClass);
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