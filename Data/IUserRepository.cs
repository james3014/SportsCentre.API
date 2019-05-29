﻿using SportsCentre.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsCentre.API.Data
{
    public interface IUserRepository
    {
        Task<IEnumerable<Staff>> GetAttendants();
    }
}
