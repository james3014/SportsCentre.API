using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SportsCentre.API.Data;
using SportsCentre.API.Dtos;
using SportsCentre.API.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace SportsCentre.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IAdminRepository repo;
        private readonly IDataRepository dataRepo;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public AdminController(DataContext context, IAdminRepository repo, IDataRepository dataRepo,
            IMapper mapper, UserManager<User> userManager)
        {
            this.context = context;
            this.repo = repo;
            this.dataRepo = dataRepo;
            this.mapper = mapper;
            this.userManager = userManager;
        }



        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userList = await (from user in context.Users
                                  orderby user.UserName
                                  select new
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      Roles = (from userRole in user.UserRoles
                                               join role in context.Roles
                                               on userRole.RoleId
                                               equals role.Id
                                               select role.Name).ToList()
                                  }).ToListAsync();

            return Ok(userList);
        }



        [HttpPost("editroles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDto roleEditDto)
        {
            var user = await userManager.FindByNameAsync(userName);

            var userRoles = await userManager.GetRolesAsync(user);

            var selectedRoles = roleEditDto.RoleNames;

            selectedRoles = selectedRoles ?? new string[] { };

            var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await userManager.GetRolesAsync(user));
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
        

        [HttpPut("classes/update/{id}")]
        public async Task<IActionResult> EditClass(int id, CreateClassDto createClassDto)
        {
            var classFromRepo = await repo.GetClass(id);

            if (classFromRepo == null) return BadRequest("No Class Found");

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

            mapper.Map(createClassDto, classFromRepo);

            if (await repo.SaveAll())
            {
                return NoContent();
            }

            throw new Exception($"Updating class {id} failed on save");
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