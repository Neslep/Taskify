using AutoMapper;
using Taskify.API.DTOs.Requests.TaskRequest;
using Taskify.API.DTOs.Responses.ProjectDTOs;
using Task = Taskify.API.Models.Task;

namespace Taskify.API.Mapper.MappingProfiles;

public class TaskMappingProfiles : Profile
{
    public TaskMappingProfiles()
    {
        CreateMap<TaskResponse, Task>().ReverseMap();
        CreateMap<CreateTaskRequest, Task>().ReverseMap();
        CreateMap<UpdateTaskRequest, Task>().ReverseMap();
    }
}