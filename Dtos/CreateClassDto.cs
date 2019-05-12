﻿using SportsCentre.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsCentre.API.Dtos
{
    public class CreateClassDto
    {
        public string ClassName { get; set; }
        public DateTime ClassDate { get; set; }
        public Staff Attendant { get; set; }
        public double Cost { get; set; }
    }
}