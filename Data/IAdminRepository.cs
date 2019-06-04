using System.Collections.Generic;
using System.Threading.Tasks;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public interface IAdminRepository
    {
        Task<Class> GetClass(int id);
        Task<IEnumerable<Class>> GetClasses();
        Task<Class> CreateNewClass(Class newClass);
        Task<IEnumerable<Booking>> GetBookings();
        Task<User> GetStaffFromUserName(string email);
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
    }
}