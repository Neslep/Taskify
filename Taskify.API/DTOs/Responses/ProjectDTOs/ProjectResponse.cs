using Taskify.API.Enums;
using Taskify.API.Models;

namespace Taskify.API.DTOs.Responses.ProjectDTOs;

public class ProjectResponse
{
    public int Id { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProjectStatus ProjectStatus { get; set; } 
    public int? OwnerId { get; set; }
    public User? Owner { get; set; }
}