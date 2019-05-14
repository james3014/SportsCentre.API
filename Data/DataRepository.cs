using System;
using System.Collections.Generic;
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

        public async Task<Booking> CreateNewBooking(Booking booking)
        {
            await context.Bookings.AddAsync(booking);
            await context.SaveChangesAsync();

            return booking;
        }

        // Classes
        public Task<Class> GetCurrentClasses()
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> CreateMembership(string plan, string email)
        {
            User user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null) return null;

            if (plan.ToLower().Equals("annual"))
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
    }
}