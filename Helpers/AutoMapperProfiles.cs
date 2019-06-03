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
        /* 
         * These are the Auto Mapper profiles that are required for mapping models to DTO's.
         * 
         * These allow staff and users to be returned to the client without compromising security.
         * They also allow for easy updating of current entities in the database.
         */
        public AutoMapperProfiles()
        {
            CreateMap<User, CurrentUserDto>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<StaffForRegisterDto, User>();
            CreateMap<User, CurrentStaffDto>();
            CreateMap<Booking, BookingDto>();
            CreateMap<CreateClassDto, Class>();
            CreateMap<StaffForRegisterDto, User>();
        }
    }
}
