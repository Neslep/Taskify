using Taskify.API.Enums;

namespace Taskify.API.DTOs.Responses.ProjectDTOs;

public class ProjectResponse
{
    public int Id { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProjectStatus ProjectStatus { get; set; } 
    public int? OwnerId { get; set; }
    public UserResponse? Owner { get; set; }
    public IEnumerable<UserProjectResponse> UserProjects { get; set; } = new List<UserProjectResponse>();
    public IEnumerable<TaskResponse> Tasks { get; set; } = new List<TaskResponse>();
    public IEnumerable<KanbanResponse> Kanbans { get; set; } = new List<KanbanResponse>();
    public IEnumerable<TodolistResponse> Todolists { get; set; } = new List<TodolistResponse>();
}

public class UserResponse
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserStatus Status { get; set; }
    public PlanType Plans { get; set; }
    // Add other properties as needed
}

public class UserProjectResponse
{
    public int UserId { get; set; }
    public int ProjectId { get; set; }
    public ProjectRole RoleInProject { get; set; }
    public UserResponse User { get; set; }
}

public class TaskResponse
{
    public int Id { get; set; }
    public string TaskName { get; set; } = string.Empty;
    // Add other properties as needed
}

public class KanbanResponse
{
    public int Id { get; set; }
    public string KanbanName { get; set; } = string.Empty;
    // Add other properties as needed
}

public class TodolistResponse
{
    public int Id { get; set; }
    public string TodolistName { get; set; } = string.Empty;
    // Add other properties as needed
}
