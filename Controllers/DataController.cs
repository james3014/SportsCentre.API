using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
            User user = await repo.GetUserFromEmail(bookingDto.Email);

            if (user == null) return null;

            switch (bookingDto.Facility)
            {
                case "1":
                    bookingDto.Facility = "Full Hall";
                    break;
                case "2":
                    bookingDto.Facility = "Half Hall";
                    break;
                case "3":
                    bookingDto.Facility = "Small Area";
                    break;
            }

            switch (bookingDto.BookingTime)
            {
                case "1":
                    bookingDto.BookingTime = "08:00 - 09:00";
                    break;
                case "2":
                    bookingDto.BookingTime = "09:00 - 10:00";
                    break;
                case "3":
                    bookingDto.BookingTime = "10:00 - 11:00";
                    break;
                case "4":
                    bookingDto.BookingTime = "11:00 - 12:00";
                    break;
                case "5":
                    bookingDto.BookingTime = "12:00 - 13:00";
                    break;
                case "6":
                    bookingDto.BookingTime = "13:00 - 14:00";
                    break;
                case "7":
                    bookingDto.BookingTime = "14:00 - 15:00";
                    break;
                case "8":
                    bookingDto.BookingTime = "15:00 - 16:00";
                    break;
                case "9":
                    bookingDto.BookingTime = "16:00 - 17:00";
                    break;
                case "10":
                    bookingDto.BookingTime = "17:00 - 18:00";
                    break;
                case "11":
                    bookingDto.BookingTime = "18:00 - 19:00";
                    break;
                case "12":
                    bookingDto.BookingTime = "19:00 - 20:00";
                    break;

            }

            Booking newbooking = new Booking
            {
                BookingEmail = bookingDto.Email,
                BookingType = bookingDto.Facility,
                BookingDate = bookingDto.BookingDate,
                BookingTime = bookingDto.BookingTime,
                Requirements = bookingDto.Requirements,
                CreatedBy = user
            };

            Booking createdBooking = await repo.CreateNewBooking(newbooking);

            return Ok(createdBooking);

        }
    }

}