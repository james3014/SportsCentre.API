using System.Collections.Generic;
using System.Threading.Tasks;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public interface IAdminRepository
    {
        Task<Class> CreateNewClass(Class newClass);
        Task<Class> EditClass();
        Task<Class> RemoveClass();
        Task<Staff> GetStaffFromEmail(string email);
    }
}