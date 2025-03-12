using Taskify.API.Enums;

namespace Taskify.API.DTOs.Requests;

public record UpdateProjectRequest(
    string ProjectName,
    string Description,
    ProjectStatus ProjectStatus,
    List<string>? MemberEmails
    );