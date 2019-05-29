using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using SportsCentre.API.Data;
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

        // Constructor
        public UserController(IUserRepository repo, IAuthRepository authRepo)
        {
            this.repo = repo;
            this.authRepo = authRepo;
        }

        [HttpGet("attendants")]
        public async Task<IActionResult> GetAttendants()
        {
            var attendants = await repo.GetAttendants();

            return Ok(attendants);
        }

    }
}