using AutoMapper;
using Taskify.API.DTOs.Requests;
using Taskify.API.DTOs.Responses.ProjectDTOs;
using Taskify.API.Models;

namespace Taskify.API.Mapper.MappingProfiles;

public class UserProjectMappingProfiles : Profile
{
    public UserProjectMappingProfiles()
    {
        CreateMap<UserProjectResponse, UserProject>().ReverseMap();
        CreateMap<AddMemberProjectRequest, UserProject>()
            .ForMember(dest => dest.ProjectId, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.LastModifiedDate, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
    
}