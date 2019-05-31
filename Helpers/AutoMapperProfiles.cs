using AutoMapper;
using SportsCentre.API.Dtos;
using SportsCentre.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsCentre.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, CurrentUserDto>();
            CreateMap<Staff, CurrentStaffDto>();
            CreateMap<Booking, BookingDto>();
            CreateMap<CreateClassDto, Class>();
            CreateMap<StaffForRegisterDto, Staff>();
        }
    }
}
