using System.Collections.Generic;
using System.Threading.Tasks;
using SportsCentre.API.Dtos;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public interface IDataRepository
    {
        Task<Booking> CreateNewBooking(Booking booking);
        Task<IEnumerable<Booking>> GetBookings();
        Task<Class> GetCurrentClasses();
        Task<Class> CreateNewClass(Class newClass);
        Task<Class> EditClass();
        Task<Class> RemoveClass();
    }
}