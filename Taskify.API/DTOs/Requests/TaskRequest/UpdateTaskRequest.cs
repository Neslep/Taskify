using Taskify.API.Enums;
using TaskStatus = Taskify.API.Enums.TaskStatus;

namespace Taskify.API.DTOs.Requests.TaskRequest;

public record UpdateTaskRequest(
    DateTime DueDate,
    PriorityLevel Priority,
    TaskStatus Status
    );