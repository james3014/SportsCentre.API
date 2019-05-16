using System.Collections.Generic;
using System.Threading.Tasks;
using SportsCentre.API.Dtos;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public interface IDataRepository
    {
        Task<User> GetUser(int id);
        Task<User> CreateMembership(CurrentUserDto currentUserDto);
        Task<Booking> CreateNewBooking(BookingDto bookingDto);
        Task<IEnumerable<Booking>> GetBookings();
        Task<Class> GetCurrentClasses();
        Task<bool> SaveAll();
    }
}