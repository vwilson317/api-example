using System;
using API.DataAccess;
using API.Dtos;
using AutoMapper;

namespace API.AutoMapper
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, FormattedUserDto>()
                .ForMember(src => src.FullName,
                opt => opt.MapFrom(o => $"{o.FristName} {o.MiddleName} {o.LastName}"));
        }
    }
}
