using AutoMapper;
using Taskify.API.DTOs.Requests;
using Taskify.API.DTOs.Responses.ProjectDTOs;
using Taskify.API.Models;
using Task = Taskify.API.Models.Task;

namespace Taskify.API.Mapper.MappingProfiles
{
    public class ProjectMappingProfile : Profile
    {
        public ProjectMappingProfile()
        {
            CreateMap<Project, UpdateProjectRequest>().ReverseMap();
            CreateMap<Project, CreateProjectRequest>().ReverseMap();
            CreateMap<Project, ProjectResponse>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner))
                .ForMember(dest => dest.UserProjects, opt => opt.MapFrom(src => src.UserProjects))
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks))
                .ForMember(dest => dest.Kanbans, opt => opt.MapFrom(src => src.Kanbans))
                .ForMember(dest => dest.Todolists, opt => opt.MapFrom(src => src.Todolists));
            CreateMap<User, UserResponse>();
            CreateMap<UserProject, UserProjectResponse>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
            CreateMap<Task, TaskResponse>();
            CreateMap<Kanban, KanbanResponse>();
            CreateMap<Todolist, TodolistResponse>();
        }
    }
}