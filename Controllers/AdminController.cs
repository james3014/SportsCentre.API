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
        // Private class variables
        private readonly DataContext context;
        private readonly IAdminRepository repo;
        private readonly IDataRepository dataRepo;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        /**
         * This public constructor is used to inject several services into the application.
         * These can then be used for accessing the corresponding repository.
         */
        public AdminController(DataContext context, IAdminRepository repo, IDataRepository dataRepo,
            IMapper mapper, UserManager<User> userManager)
        {
            this.context = context;
            this.repo = repo;
            this.dataRepo = dataRepo;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        /**
         * This function is used to provide the role management panel with details of all users and roles.
         * A LINQ query is ran with a join to bring together both the Users and Roles tables from Identity.
         */
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


        /**
         * This function provides an update end point for the edit roles feature of the administrator.
         * The function finds the user via the username parameter and then finds all current roles for the user.
         * The new roles are then allocated as part of a string array to the user and the new roles are returned.
         */
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
    


        /**
         * This function is used by administrators to create a new class for the leisure center.
         * It initially finds the attendant who will be running the class via their username.
         * After which a new class is created with detailed from the data transfer object.
         */
        [HttpPost("classes/create")]
        public async Task<IActionResult> CreateNewClass(CreateClassDto createClassDto)
        {
            var staff = await repo.GetStaffFromUserName(createClassDto.AttendantUserName);

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
                User = staff,
                Cost = createClassDto.Cost
            };

            Class createdClass = await repo.CreateNewClass(newClass);

            return Ok(createdClass);
        }
        
        
        /**
         * This function is used to provide a class update end point for the administrator.
         * It takes the createClassDto and finds the current class from it's ID. The attendant 
         * is then found via their username and automapper is used to map the updated details to the 
         * current class
         */
        [HttpPut("classes/update/{id}")]
        public async Task<IActionResult> EditClass(int id, CreateClassDto createClassDto)
        {
            var classFromRepo = await repo.GetClass(id);

            if (classFromRepo == null) return BadRequest("No Class Found");

            var staff = await repo.GetStaffFromUserName(createClassDto.AttendantUserName);

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

        /**
         * This function is used to delete a class from the database.
         * A class ID is passed from the client which allows the class to be found.
         * Once found the delete entity function inside the corresponding repository is called.
         */
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