using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public class Seed
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public void SeedUsers()
        {
            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            var roles = new List<Role>
            {
                new Role{Name = "User"},
                new Role{Name = "Member"},
                new Role{Name = "Staff"},
                new Role{Name = "Admin"},
                new Role{Name = "Attendant"}
            };

            foreach (var role in roles)
            {
                roleManager.CreateAsync(role).Wait();
            }

            foreach (var user in users)
            {
                userManager.CreateAsync(user, "password").Wait();
                userManager.AddToRoleAsync(user, "Member");
            }

            var adminUser = new User
            {
                UserName = "Admin"
            };

            IdentityResult result = userManager.CreateAsync(adminUser, "password").Result;

            if (result.Succeeded)
            {
                var admin = userManager.FindByNameAsync("Admin").Result;
                userManager.AddToRolesAsync(admin, new[] { "Admin" }).Wait();
            }
        }
    }
}