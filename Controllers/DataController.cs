using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> userManager;


        /*
        * This public constructor is used to inject several services into the application.
        * These can then be used for accessing the corresponding repository.
        */
        public DataController(IDataRepository repo, IMapper mapper, UserManager<User> userManager)
        {
            this.repo = repo;
            this.mapper = mapper;
            this.userManager = userManager;
        }


        /*
         * This function is used to return a specific user via their unique ID.
         * If the user is found it is mapped to a DTO which will not include their security information.
         */
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        { 
            var user = await repo.GetUser(id);

            var userToReturn = mapper.Map<CurrentUserDto>(user);

            return Ok(userToReturn);
        }


        /*
         * This function is used to create a new membership for a current user.
         * If a user decides to purchase a membership their role is changed to Member.
         */
        [HttpPost("membership/create")]
        public async Task<IActionResult> CreateMembership(CurrentUserDto currentUserDto)
        {
            User user = await repo.CreateMembership(currentUserDto);

            await userManager.AddToRoleAsync(user, "Member");

            if (await repo.SaveAll())
            {
                return NoContent();
            }

           throw new Exception($"Updating user {user.Id} failed on save");
        }


        /*
         * This function is used cancel a users membership.
         * The user ID is passed from the client which enables the user to be found.
         * Once this is done their member role is removed from the database.
         */
        [HttpGet("membership/cancel/{id}")]
        public async Task<IActionResult> CancelMembership(int id)
        {
            User user = await repo.GetUser(id);

            if (user == null) return BadRequest("User does not exist");

            await userManager.RemoveFromRoleAsync(user, "Member");

            user.MembershipType = "";
            user.MembershipExpiry = DateTime.Now;

            if (await repo.SaveAll())
            {
                return NoContent();
            }

            throw new Exception($"Cancelling membership failed on save");
        }


        /*
         * This function is used to retrieve all bookings from the database.
         * The GetBookings method inside the corresponding repo retreieves the data
         * from the database before returning this back to the client.
         */
        [HttpGet("bookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await repo.GetBookings();

            return Ok(bookings);
        }


        /*
         * This function allows for a specific users bookings to be returned.
         * The ID of the user is sent to the server and the GetUserBookings 
         * method is used to retrieve all bookings from the database.
         */
        [HttpGet("bookings/{id}")]
        public async Task<IActionResult> GetUserBookings(int id)
        {
            var bookings = await repo.GetUserBookings(id);

            return Ok(bookings);
        }


        /*
         * This function is used to cancel a specific booking for a user.
         * The client will pass the booking ID which allows the repo to find it.
         * If the booking is for a class then the attendance will be lowered by 1.
         * Otherwise the booking is simply deleted via the delete function.
         */
        [HttpDelete("bookings/cancel/{id}")]
        public async Task<IActionResult> CancelUserBooking(int id)
        {
            var booking = await repo.GetBooking(id);

            if (booking == null)
            {
                return BadRequest("Booking does not exist");
            }
            else if (booking.BookingType.Equals("Class"))
            {
                var selectedClass = await repo.GetClass(booking.Class.Id);

                selectedClass.TotalAttendees--;

                repo.Delete(booking);
            }
            else
            {
                repo.Delete(booking);
            }


            if (await repo.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Failed to delete booking");
        }


        /*
         * This function is used to get all current classes.
         * The corresponding repo function is used to search the database
         * and returns all classes back to the client.
         */
        [HttpGet("classes")]
        public async Task<IActionResult> GetClasses()
        {
            var currentClasses = await repo.GetClasses();

            return Ok(currentClasses);
        }


        /*
         * This function allows the client to book a current running class.
         * The class ID and booking DTO are passed from the client. The class is found via
         * the repo and it's attendance is increased by 1. Once the user is found a new booking
         * object is created and passed to the repo to create the booking.
         */
        [HttpPost("bookings/classes/{id}")]
        public async Task<IActionResult> BookClass(int id, ClassBookingDto classBookingDto)
        {
            Class selectedClass = await repo.GetClass(id);

            if (selectedClass == null)
            {
                return BadRequest("Class Not Found");
            }
            else
            {
                selectedClass.TotalAttendees++;
            }
           
            User user = await repo.GetUserFromEmail(classBookingDto.Email);

            if (user == null) return BadRequest("Email Not Registered");

            Booking newbooking = new Booking
            {
                BookingEmail = user.Email,
                FacilityType = selectedClass.Facility,
                ContactNumber = classBookingDto.ContactNumber,
                BookingType = "Class",
                Class = selectedClass,
                BookingDate = selectedClass.ClassDate,
                BookingTime = selectedClass.ClassTime,
                Requirements = classBookingDto.Requirements,
                CreatedBy = user
            };

            Booking createdBooking = await repo.CreateNewBooking(newbooking);

            return Ok(createdBooking);


        }

        /*
         * This function is used to provide the client a method for creating a facility
         * booking. The bookingDto is used to provide all required information. The user 
         * is located via the repo as well as all current bookings. 
         * 
         * A conditional check is ran to avoid duplicate bookings of the full hall on the
         * same date at the same time.
         * 
         * If this passes then a new booking is created for the user.
         */
        [HttpPost("bookings/facility")]
        public async Task<IActionResult> CreateNewBooking(BookingDto bookingDto)
        {
            DateTime bookingLimit = DateTime.Now.AddDays(8);

            User user = await repo.GetUserFromEmail(bookingDto.Email);

            var allCurrentBookings = await repo.GetBookings();

            if (user == null) BadRequest("Email Not Registered");

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

            if (bookingDto.BookingDate > bookingLimit)
            {
                return BadRequest("Bookings cannot be made more than 8 days in advance");
            }
            else if (bookingDto.Facility == "Full Hall")
            {
                foreach (var item in allCurrentBookings)
                {
                    if (item.BookingDate.ToShortDateString().Equals(bookingDto.BookingDate.ToShortDateString()) && item.BookingTime.Equals(bookingDto.BookingTime))
                    {
                        return BadRequest("Time Slot Taken");
                    }
                }
            }

            Booking newbooking = new Booking
            {
                BookingEmail = bookingDto.Email,
                FacilityType= bookingDto.Facility,
                BookingType = "Facility",
                BookingDate = bookingDto.BookingDate,
                BookingTime = bookingDto.BookingTime,
                Requirements = bookingDto.Requirements,
                CreatedBy = user
            };

            Booking createdBooking = await repo.CreateNewBooking(newbooking);

            return Ok(createdBooking);
        }

        /*
         * This function is used to create a new function booking. The function DTO is passed
         * from the client and the user is found. A new booking object is created with all relevant
         * information and this is then passed to the repo to add to the database.
         */
        [HttpPost("bookings/function")]
        public async Task<IActionResult> BookFunction(FunctionBookingDto functionBookingDto)
        {
            User user = await repo.GetUserFromEmail(functionBookingDto.Email);

            if (user == null) BadRequest("Email Address Not Registered");

            DateTime bookingLimit = DateTime.Now.AddDays(30);

            if (functionBookingDto.BookingDate > bookingLimit)
            {
                return BadRequest("Bookings cannot be made more than 30 days in advance");
            }

            Booking newbooking = new Booking
            {
                BookingEmail = functionBookingDto.Email,
                ContactNumber = functionBookingDto.ContactNumber,
                FacilityType = "Function Suite",
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