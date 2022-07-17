using Application.Models.Dtos.Admin;
using AutoMapper;
using Domain.Entities.Admin;

namespace Application.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserDto>();
    }
}