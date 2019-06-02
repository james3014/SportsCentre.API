using System.Collections.Generic;
using System.Threading.Tasks;
using SportsCentre.API.Dtos;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public interface IDataRepository
    {
        Task<User> GetUser(int id);
        Task<User> GetUserFromEmail(string email);
        Task<IEnumerable<Class>> GetClasses();
        Task<Class> GetClass(int id);
        Task<IEnumerable<Class>> GetStaffClasses(Staff staffFromRepo);
        Task<User> CreateMembership(CurrentUserDto currentUserDto);
        Task<Booking> CreateNewBooking(Booking newBooking);
        Task<IEnumerable<Booking>> GetBookings();
        Task<bool> SaveAll();
    }
}