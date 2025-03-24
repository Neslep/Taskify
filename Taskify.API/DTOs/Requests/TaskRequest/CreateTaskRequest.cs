using Taskify.API.Enums;
using TaskStatus = Taskify.API.Enums.TaskStatus;

namespace Taskify.API.DTOs.Requests.TaskRequest;

public record CreateTaskRequest(
    string TaskName,
    string AssignedEmail,
    DateTime DueDate,
    PriorityLevel Priority,
    TaskStatus Status
);
