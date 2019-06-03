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


        public async Task<IEnumerable<User>> GetAttendants()
        {
            var attendants = await userManager.GetUsersInRoleAsync("Attendant");

            return attendants;
        }

        public async Task<IEnumerable<User>> GetStaff()
        {
            var allStaff = await userManager.GetUsersInRoleAsync("Staff");

            return allStaff;
        }

        public async Task<User> GetUser(int id)
        {
            var query = context.Users.AsQueryable();

            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }
    }


}
