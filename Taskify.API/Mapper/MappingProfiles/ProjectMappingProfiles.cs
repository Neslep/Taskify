using AutoMapper;
using Taskify.API.DTOs.Requests;
using Taskify.API.Models;

namespace Taskify.API.Mapper.MappingProfiles
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, UpdateProjectRequest>().ReverseMap();
            CreateMap<Project, CreateProjectRequest>().ReverseMap();
        }
    }
}