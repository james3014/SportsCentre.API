using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly UserManager<User> userManager;

        public UserRepository(DataContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }


        /*
         * This function is used to return all staff members with the Attendant role
         * for use in creating a new class. The context is used to complete a query and 
         * all attendants are returned.
         */
        public async Task<IEnumerable<User>> GetAttendants()
        {
            var attendants = await userManager.GetUsersInRoleAsync("Attendant");

            return attendants;
        }

        /* This function is used to retrieve all staff with the staff role from the database.
         * User Manager is used to complete this and the collection of staff is then returned.
         */
        public async Task<IEnumerable<User>> GetStaff()
        {
            var allStaff = await userManager.GetUsersInRoleAsync("Staff");

            return allStaff;
        }

        /*
         * This function is used to get a specific user from the database.
         * The user ID is passed from the controller and this is assigned to a query.
         * The query is then used to locate the user and return them.
         */
        public async Task<User> GetUser(int id)
        {
            var query = context.Users.AsQueryable();

            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }
    }


}
