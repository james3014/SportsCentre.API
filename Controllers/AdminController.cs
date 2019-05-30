using System;
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
        private readonly IDataRepository dataRepo;
        private readonly IConfiguration config;

        public AdminController(IAdminRepository repo, IConfiguration config, IDataRepository dataRepo)
        {
            this.repo = repo;
            this.config = config;
            this.dataRepo = dataRepo;
        }


        [HttpPost("classes/create")]
        public async Task<IActionResult> CreateNewClass(CreateClassDto createClassDto)
        {
            var staff = await repo.GetStaffFromEmail(createClassDto.AttendantEmail);

            if (staff == null) return null;

            switch (createClassDto.Facility)
            {
                case "1":
                    createClassDto.Facility = "Full Hall";
                    break;
                case "2":
                    createClassDto.Facility = "Half Hall";
                    break;
                case "3":
                    createClassDto.Facility = "Small Area";
                    break;
            }

            switch (createClassDto.ClassTime)
            {
                case "1":
                    createClassDto.ClassTime = "08:00 - 09:00";
                    break;
                case "2":
                    createClassDto.ClassTime = "09:00 - 10:00";
                    break;
                case "3":
                    createClassDto.ClassTime = "10:00 - 11:00";
                    break;
                case "4":
                    createClassDto.ClassTime = "11:00 - 12:00";
                    break;
                case "5":
                    createClassDto.ClassTime = "12:00 - 13:00";
                    break;
                case "6":
                    createClassDto.ClassTime = "13:00 - 14:00";
                    break;
                case "7":
                    createClassDto.ClassTime = "14:00 - 15:00";
                    break;
                case "8":
                    createClassDto.ClassTime = "15:00 - 16:00";
                    break;
                case "9":
                    createClassDto.ClassTime = "16:00 - 17:00";
                    break;
                case "10":
                    createClassDto.ClassTime = "17:00 - 18:00";
                    break;
                case "11":
                    createClassDto.ClassTime = "18:00 - 19:00";
                    break;
                case "12":
                    createClassDto.ClassTime = "19:00 - 20:00";
                    break;

            }

            Class newClass = new Class
            {
                ClassName = createClassDto.ClassName,
                ClassDate = createClassDto.ClassDate,
                ClassTime = createClassDto.ClassTime,
                Facility = createClassDto.Facility,
                MaxAttendees = createClassDto.MaxAttendees,
                TotalAttendees = 0,
                Attendant = staff,
                Cost = createClassDto.Cost
            };

            Class createdClass = await repo.CreateNewClass(newClass);

            return Ok(createdClass);
        }

        
        [HttpPut("editclass")]
        public Task<IActionResult> EditClass(CreateClassDto editClassDto)
        {
            throw new System.Exception();
        }


        [HttpDelete("classes/delete{id}")]
        public async Task<IActionResult> RemoveClass(int id)
        {
            var classFromRepo = await repo.GetClass(id);

            if (classFromRepo == null) return BadRequest("Class does not exist");

            repo.Delete(classFromRepo);

            if (await repo.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Failed to delete class");
        }
    } 
}