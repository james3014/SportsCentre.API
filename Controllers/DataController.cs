using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SportsCentre.API.Data;
using SportsCentre.API.Dtos;
using SportsCentre.API.Models;
using System;
using System.Threading.Tasks;

namespace SportsCentre.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataRepository repo;
        private readonly IMapper mapper;

        public DataController(IDataRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        { 
            var user = await repo.GetUser(id);

            var userToReturn = mapper.Map<CurrentUserDto>(user);

            return Ok(userToReturn);
        }

        [HttpPost("membership/create")]
        public async Task<IActionResult> CreateMembership(CurrentUserDto currentUserDto)
        {
            User user = await repo.CreateMembership(currentUserDto);

            if (await repo.SaveAll())
            {
                return NoContent();
            }

           throw new Exception($"Updating user {user.Id} failed on save");
        }


        [HttpGet("bookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await repo.GetBookings();

            return Ok(bookings);
        }


        [HttpPost("bookings/create")]
        public async Task<IActionResult> CreateNewBooking(BookingDto bookingDto)
        {
            Booking booking = await repo.CreateNewBooking(bookingDto);


        }
    }

}