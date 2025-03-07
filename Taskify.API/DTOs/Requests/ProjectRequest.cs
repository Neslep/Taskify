using Taskify.API.Enums;

namespace Taskify.API.DTOs.Requests;

public record ProjectRequest(
    string ProjectName,
    string Description,
    ProjectStatus ProjectStatus
    );