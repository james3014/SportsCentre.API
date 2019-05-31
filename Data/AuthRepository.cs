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

        public async Task<User> Login(string email, string password)
        {
            User user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null) return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return null;

            return user;
        }

        public async Task<Staff> StaffLogin(string email, string password)
        {
            Staff staff = await context.Staff.FirstOrDefaultAsync(x => x.Email == email);

            if (staff == null) return null;

            if (!VerifyPasswordHash(password, staff.PasswordHash, staff.PasswordSalt)) return null;

            return staff;
        }



        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }

            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<Staff> CreateStaff(Staff staff, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            staff.PasswordHash = passwordHash;
            staff.PasswordSalt = passwordSalt;

            await context.Staff.AddAsync(staff);
            await context.SaveChangesAsync();

            return staff;
        }

        public async Task<Staff> GetStaff(int id)
        {
            var staffMember = await context.Staff.FirstOrDefaultAsync(s => s.Id == id);

            return staffMember;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string email)
        {
            if (await context.Users.AnyAsync(x => x.Email == email)) return true;

            return false;
        }

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