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

        /*
         * This function is used to retrieve classes from the database via their ID.
         */
        public async Task<Class> GetClass(int id)
        {
            var returnedClass = await context.Classes.FirstOrDefaultAsync(c => c.Id == id);

            return returnedClass;
        }

        /* This function is used to create a new class inside the database.
         * A class object is passed via the controller and this is then added
         * to the database. Once added the changes are saved.
         */
        public async Task<Class> CreateNewClass(Class newClass)
        {
            await context.Classes.AddAsync(newClass);
            await context.SaveChangesAsync();

            return newClass;
        }

        /*
         * This function is used to retrieve a specific staff member from the database.
         * The Users table is searched using the username parameter and this is then returned.
         */
        public async Task<User> GetStaffFromUserName(string userName)
        {
            var staff = await context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            return staff;
        }

        /*
         * This function takes a generic and is used to delete an entity from the database.
         */
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