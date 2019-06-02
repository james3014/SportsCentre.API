using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportsCentre.API.Dtos;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public class DataRepository : IDataRepository
    {
        // Properties
        private readonly DataContext context;


        // Constructor
        public DataRepository(DataContext context)
        {
            this.context = context;
        }


        // Bookings
        public async Task<IEnumerable<Booking>> GetBookings()
        {
            var bookings = await context.Bookings.ToListAsync();

            return bookings;
        }

        public async Task<Booking> CreateNewBooking(Booking newBooking)
        {
            await context.Bookings.AddAsync(newBooking);
            await context.SaveChangesAsync();

            return newBooking;
        }

        // Classes
        public async Task<IEnumerable<Class>> GetClasses()
        {
            var currentClasses = await context.Classes.ToListAsync();

            return currentClasses;
        }

        public async Task<Class> GetClass(int id)
        {
            var selectedClass = await context.Classes.FirstOrDefaultAsync(c => c.Id == id);

            return selectedClass;
        }

        public async Task<IEnumerable<Class>> GetStaffClasses(Staff staffFromRepo)
        {
            var selectedStaffClasses = await context.Classes.Where(s => s.Attendant.Id == staffFromRepo.Id).ToListAsync();

            context.RemoveRange(selectedStaffClasses);
            await context.SaveChangesAsync();

            return selectedStaffClasses;
        }

        public async Task<User> CreateMembership(CurrentUserDto currentUserDto)
        {
            User user = await context.Users.FirstOrDefaultAsync(x => x.Id == currentUserDto.Id);

            if (user == null) return null;

            if (currentUserDto.MembershipType.ToLower().Equals("annual"))
            {
                user.MembershipType = "Annual";
                user.MembershipExpiry = DateTime.Now.AddYears(1);
            }
            else
            {
                user.MembershipType = "Monthly";
                user.MembershipExpiry = DateTime.Now.AddMonths(1);
            }

            return user;
        }


         // Users
        public async Task<User> GetUser(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetUserFromEmail(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        // Save All Changes
        public async Task<bool> SaveAll()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}