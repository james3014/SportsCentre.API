﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportsCentre.API.Models;

namespace SportsCentre.API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;


        public UserRepository(DataContext context)
        {
            this.context = context;
        }


        public async Task<IEnumerable<Staff>> GetAttendants()
        {
            var staff = await context.Staff.ToListAsync();

            var attendants = staff.Where(o => o.Role.Equals("Attendant"));

            return attendants;
        }
    }


}