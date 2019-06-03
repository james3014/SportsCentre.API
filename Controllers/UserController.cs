using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using SportsCentre.API.Data;
using SportsCentre.API.Dtos;
using SportsCentre.API.Models;

namespace SportsCentre.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository repo;
        private readonly IAuthRepository authRepo;
        private readonly IMapper mapper;


        /*
        * This public constructor is used to inject several services into the application.
        * These can then be used for accessing the corresponding repository.
        */
        public UserController(IUserRepository repo, IAuthRepository authRepo, IMapper mapper)
        {
            this.repo = repo;
            this.authRepo = authRepo;
            this.mapper = mapper;
        }


        /*
         * This function is used by the administrator to load attendants to man classes.
         * The function makes use of the repo method GetAttendants which returns all staff
         * with the attendant role.
         */
        [HttpGet("attendants")]
        public async Task<IActionResult> GetAttendants()
        {
            var attendants = await repo.GetAttendants();

            return Ok(attendants);
        }


        /*
         * This function is used to return all staff back to the client.
         * The GetStaff method inside the corresponding repo is used to return 
         * all users with the staff role.
         */
        [HttpGet("staff")]
        public async Task<IActionResult> GetStaff()
        {
            var allStaff = await repo.GetStaff();

            return Ok(allStaff);
        }


        /*
         * This function is used to retrieve a specific user from the database.
         * The user ID is used by the GetUser function to locate them in the database.
         * The user is then mapped to a secure DTO for returning.
         */
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var isCurrentUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) == id;

            var user = await repo.GetUser(id);

            var userToReturn = mapper.Map<UserForDetailedDto>(user);

            return Ok(userToReturn);
        }

    }
}