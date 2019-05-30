using System.Threading.Tasks;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<Staff> GetStaff(int id);
         Task<Staff> CreateStaff(Staff staff, string password);
         Task<User> Login(string email, string password);
         Task<Staff> StaffLogin(string email, string password);
         Task<bool> UserExists(string email);
         void Delete<T>(T entity) where T : class;
         Task<bool> SaveAll();
    }
}