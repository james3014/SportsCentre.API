using System.Threading.Tasks;
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


        public async Task<Class> CreateNewClass(Class newClass)
        {
            await context.Classes.AddAsync(newClass);
            await context.SaveChangesAsync();

            return newClass;
        }

        public Task<Class> EditClass()
        {
            throw new System.NotImplementedException();
        }

        public Task<Class> RemoveClass()
        {
            throw new System.NotImplementedException();
        }
    }
}