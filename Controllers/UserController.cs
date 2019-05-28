using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using SportsCentre.API.Data;

namespace SportsCentre.API.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // Private variables
        private readonly IUserRepository repo;
        private readonly IConfiguration config;
        private readonly IMapper mapper;


        // Constructor
        public UserController(IUserRepository repo, IConfiguration config, IMapper mapper)
        {
            this.config = config;
            this.mapper = mapper;
            this.repo = repo;
        }

        [HttpGet("staff/attendants")]
        public async Task<IActionResult> GetAttendants()
        {

        }
    }
}