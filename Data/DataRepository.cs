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
        private readonly DataContext context;

        public DataRepository(DataContext context)
        {
            this.context = context;
        }


        /*
         * This function uses context to retrieve all current bookings from the database.
         */
        public async Task<IEnumerable<Booking>> GetBookings()
        {
            var bookings = await context.Bookings.ToListAsync();

            return bookings;
        }

        /*
         * This function is used to get a specific users bookings from the database.
         * A user ID is passed as a parameter and this is used within the function.
         */
        public async Task<IEnumerable<Booking>> GetUserBookings(int id)
        {
            var userBookings = await context.Bookings.Where(i => i.CreatedBy.Id == id).ToListAsync();

            return userBookings;
        }

        /*
         * This function is used to retrieve a specific booking from the database.
         * The booking ID is passed as a parameter from the controller and is then returned.
         */
        public async Task<Booking> GetBooking(int id)
        {
            var booking = await context.Bookings.Include(b => b.Class).FirstOrDefaultAsync(i => i.Id == id);

            return booking;
        }

        /*
         * This function is used to create a new booking. The booking object is passed
         * from the controller and the context methods are used to add this to the database.
         */
        public async Task<Booking> CreateNewBooking(Booking newBooking)
        {
            await context.Bookings.AddAsync(newBooking);
            await context.SaveChangesAsync();

            return newBooking;
        }

        
        /*
         * This function is used to retrieve all current classes from the database.
         * These are then passed as a list object back to the controller.
         */
        public async Task<IEnumerable<Class>> GetClasses()
        {
            var currentClasses = await context.Classes.ToListAsync();

            return currentClasses;
        }

        /*
         * This function is used to get a specific class from the database. A class ID
         * is passed to the function and context is used to find it. Once found this is then
         * returned back to the controller.
         */
        public async Task<Class> GetClass(int id)
        {
            var selectedClass = await context.Classes.FirstOrDefaultAsync(c => c.Id == id);

            return selectedClass;
        }

        /*
         * This function is used to get a specific staff members current classes.
         * The user object is passed and from this their ID is used to find their currently 
         * associated classes. These are then removed from the database using the remove range function.
         * 
         * This function is called when a staff member is being removed from the database.
         */
        public async Task<IEnumerable<Class>> GetStaffClasses(User staffFromRepo)
        {
            var selectedStaffClasses = await context.Classes.Where(s => s.User.Id == staffFromRepo.Id).ToListAsync();

            context.RemoveRange(selectedStaffClasses);
            await context.SaveChangesAsync();

            return selectedStaffClasses;
        }

        /*
         * This function is used to create a new membership for a user. The DTO includes
         * their ID which is used to find them in the database.
         * 
         * Once found their membership expiry and type is updated to the relevant one.
         */
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


         /*
          * This function is used to return a specific user from the database.
          * There ID is passed to the function which allows a context search to 
          * be completed and the user is then returned.
          */
        public async Task<User> GetUser(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        /* This function is used to find a user via their registered email address.
         * The email is passed as a parameter which allows the search to be completed.
         * The user is then returned.
         */
        public async Task<User> GetUserFromEmail(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        /*
        * This function takes a generic and is used to delete an entity from the database.
        */
        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }


        /*
         This function is used to save any changes made to the database.
         */
        public async Task<bool> SaveAll()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}