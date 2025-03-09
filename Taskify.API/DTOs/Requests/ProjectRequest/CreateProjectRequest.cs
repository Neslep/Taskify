using Taskify.API.Enums;

namespace Taskify.API.DTOs.Requests;

public record CreateProjectRequest(
    string ProjectName,
    string Description,
    ProjectStatus ProjectStatus,
    List<int>? MemberIds
    
    );