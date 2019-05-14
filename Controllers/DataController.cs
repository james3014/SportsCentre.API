using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SportsCentre.API.Data;
using SportsCentre.API.Dtos;
using SportsCentre.API.Models;
using System.Threading.Tasks;

namespace SportsCentre.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataRepository repo;
        private readonly IConfiguration config;


        public DataController(IDataRepository repo, IConfiguration config)
        {
            this.repo = repo;
            this.config = config;
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await repo.GetUser(id);

            return Ok(user);
        }

        [HttpPost("membership/create")]
        public async Task<IActionResult> CreateMembership(string type, string email)
        {
            User user = await repo.CreateMembership(type, email);

            return Ok(user);
        }


        [HttpGet("bookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await repo.GetBookings();

            return Ok(bookings);
        }


        [HttpPost("bookings/create")]
        public async Task<IActionResult> CreateNewBooking(Booking booking)
        {
            Booking newBooking = new Booking
            {
                BookingName = booking.BookingName,
                BookingDate = booking.BookingDate,
                CreatedBy = booking.CreatedBy,
                BookingType = booking.BookingType,
                PaymentDetail = booking.PaymentDetail
            };

            Booking createdBooking = await repo.CreateNewBooking(newBooking);

            return Ok(createdBooking);
        }
    }

}