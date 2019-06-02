using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext context;


        public AdminRepository(DataContext context)
        {
            this.context = context;
        }


        public async Task<Class> GetClass(int id)
        {
            var returnedClass = await context.Classes.FirstOrDefaultAsync(c => c.Id == id);

            return returnedClass;
        }

        public async Task<Class> CreateNewClass(Class newClass)
        {
            await context.Classes.AddAsync(newClass);
            await context.SaveChangesAsync();

            return newClass;
        }

        public async Task<Staff> GetStaffFromEmail(string email)
        {
            var staff = await context.Staff.FirstOrDefaultAsync(u => u.Email == email);

            return staff;
        }

        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}