using System.Collections.Generic;
using System.Threading.Tasks;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public interface IAdminRepository
    {
        Task<Class> GetClass(int id);
        Task<Class> CreateNewClass(Class newClass);
        Task<Staff> GetStaffFromEmail(string email);
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
    }
}