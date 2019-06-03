using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;

        public AuthRepository(DataContext context)
        {
            this.context = context;
        }

        /*
         * This function allows users to login to the system.
         * It takes a username and password and then looks to find the user in the database.
         * If found it returns the user back to the controller.
         * 
         * Previously the function also completed a password hash verification but this is now
         * handled by Identity instead.
         */
        public async Task<User> Login(string userName, string password)
        {
            User user = await context.Users.FirstOrDefaultAsync(x => x.UserName == userName);

            if (user == null) return null;

            //if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return null;

            return user;
        }



        //private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        //{
        //    using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        //    {
        //        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        //        for (int i = 0; i < computedHash.Length; i++)
        //        {
        //            if (computedHash[i] != passwordHash[i]) return false;
        //        }
        //    }

        //    return true;
        //}



        /*
        * This function is used to register a new user to the database.
        * It takes a user object and password which are passed to the context method.
        * 
        * Previously the password hash was output in this function but this is now
        * handled by identity Framework.
        */
        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            //user.PasswordHash = passwordHash;
            //user.PasswordSalt = passwordSalt;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        //public async Task<User> CreateStaff(User staff, string password)
        //{
        //    byte[] passwordHash, passwordSalt;
        //    CreatePasswordHash(password, out passwordHash, out passwordSalt);

        //    staff.PasswordHash = passwordHash;
        //    staff.PasswordSalt = passwordSalt;

        //    await context.Staff.AddAsync(staff);
        //    await context.SaveChangesAsync();

        //    return staff;
        //}


        /*
            * This function is used to retrieve specific staff members from the databse.
            * It takes an ID parameter and passes this to the context method.
            * This is then returned to the controller.
            */
        public async Task<User> GetStaff(int id)
        {
            var staffMember = await context.Users.FirstOrDefaultAsync(s => s.Id == id);

            return staffMember;
        }

        /*
         * This function is used to create a password hash for the user. The password is passed in and using
         * a system cryptography library a hash and salt are generated. These are these passed out as byte arrays.
         */
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        /*
        * This function is used to confirm that the user exists within the database.
        * The user email is passed from the controller and context is used to find them.
        * If found a boolean is returned to the controller.
        */
        public async Task<bool> UserExists(string email)
        {
           if (await context.Users.AnyAsync(x => x.Email == email)) return true;

            return false;
        }


        /*
        * This function takes a generic and is used to delete an entity from the database.
        */
        public void Delete<T>(T entity) where T : class
        {
            context.Remove(entity);
        }


        /*
         * This function is used to save any changes made to the database.
         */
        public async Task<bool> SaveAll()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}