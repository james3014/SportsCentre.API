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
        // Private variables
        private readonly IUserRepository repo;
        private readonly IAuthRepository authRepo;
        private readonly IMapper mapper;

        // Constructor
        public UserController(IUserRepository repo, IAuthRepository authRepo, IMapper mapper)
        {
            this.repo = repo;
            this.authRepo = authRepo;
            this.mapper = mapper;
        }

        [HttpGet("attendants")]
        public async Task<IActionResult> GetAttendants()
        {
            var attendants = await repo.GetAttendants();

            return Ok(attendants);
        }

        [HttpGet("staff")]
        public async Task<IActionResult> GetStaff()
        {
            var allStaff = await repo.GetStaff();

            return Ok(allStaff);
        }

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