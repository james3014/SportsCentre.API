using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportsCentre.API.Dtos;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly DataContext context;

        public DataRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Booking>> GetBookings()
        {
            var bookings = await context.Bookings.ToListAsync();

            return bookings;
        }


        public async Task<Class> CreateNewClass(Class newClass)
        {
            await context.Classes.AddAsync(newClass);
            await context.SaveChangesAsync();

            return newClass;
        }


        public Task<Class> EditClass()
        {
            
        }


        public Task<Class> GetCurrentClasses()
        {
            throw new System.NotImplementedException();
        }


        public Task<Class> RemoveClass()
        {
            throw new System.NotImplementedException();
        }
    }
}