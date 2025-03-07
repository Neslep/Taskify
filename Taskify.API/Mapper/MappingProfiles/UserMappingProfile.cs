using AutoMapper;
using Taskify.API.DTOs.Requests;
using Taskify.API.DTOs.Responses.UserDTOs;
using Taskify.API.Models;

namespace Taskify.API.Mapper.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<User, CreateUserRequest>().ReverseMap();
        }
    }
}
