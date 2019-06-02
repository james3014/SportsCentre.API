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

        [HttpGet("membership/cancel/{id}")]
        public async Task<IActionResult> CancelMembership(int id)
        {
            User user = await repo.GetUser(id);

            if (user == null) return BadRequest("User does not exist");

            user.MembershipType = "";
            user.MembershipExpiry = DateTime.Now;

            if (await repo.SaveAll())
            {
                return NoContent();
            }

            throw new Exception($"Cancelling membership failed on save");
        }


        [HttpGet("bookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await repo.GetBookings();

            return Ok(bookings);
        }

        [HttpGet("bookings/{id}")]
        public async Task<IActionResult> GetUserBookings(int id)
        {
            var bookings = await repo.GetUserBookings(id);

            return Ok(bookings);
        }

        [HttpDelete("bookings/cancel/{id}")]
        public async Task<IActionResult> CancelUserBooking(int id)
        {
            var booking = await repo.GetBooking(id);

            if (booking == null) return BadRequest("Booking does not exist");

            repo.Delete(booking);

            if (await repo.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Failed to delete booking");
        }



        [HttpGet("classes")]
        public async Task<IActionResult> GetClasses()
        {
            var currentClasses = await repo.GetClasses();

            return Ok(currentClasses);
        }

        [HttpPost("bookings/classes/{id}")]
        public async Task<IActionResult> BookClass(int id, ClassBookingDto classBookingDto)
        {
            Class selectedClass = await repo.GetClass(id);

            if (selectedClass == null) return null;

            User user = await repo.GetUserFromEmail(classBookingDto.Email);

            if (user == null) return null;

            Booking newbooking = new Booking
            {
                BookingEmail = user.Email,
                Facility = selectedClass.Facility,
                ContactNumber = classBookingDto.ContactNumber,
                BookingType = "Class",
                BookingDate = selectedClass.ClassDate,
                BookingTime = selectedClass.ClassTime,
                Requirements = classBookingDto.Requirements,
                CreatedBy = user
            };

            Booking createdBooking = await repo.CreateNewBooking(newbooking);

            return Ok(createdBooking);


        }


        [HttpPost("bookings/facility")]
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
                Facility = bookingDto.Facility,
                BookingType = "Facility",
                BookingDate = bookingDto.BookingDate,
                BookingTime = bookingDto.BookingTime,
                Requirements = bookingDto.Requirements,
                CreatedBy = user
            };

            Booking createdBooking = await repo.CreateNewBooking(newbooking);

            return Ok(createdBooking);
        }

        [HttpPost("bookings/function")]
        public async Task<IActionResult> BookFunction(FunctionBookingDto functionBookingDto)
        {
            User user = await repo.GetUserFromEmail(functionBookingDto.Email);

            if (user == null) return null;

            Booking newbooking = new Booking
            {
                BookingEmail = functionBookingDto.Email,
                ContactNumber = functionBookingDto.ContactNumber,
                Facility = "Function Suite",
                Attendees = functionBookingDto.Attendees,
                BookingType = "Function",
                BookingDate = functionBookingDto.BookingDate,
                BookingTime = "Full Day",
                Requirements = functionBookingDto.Requirements,
                CreatedBy = user
            };

            Booking createdBooking = await repo.CreateNewBooking(newbooking);

            return Ok(createdBooking);
        }
    }

}