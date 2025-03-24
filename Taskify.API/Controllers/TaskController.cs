using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskify.API.DTOs.Requests.TaskRequest;
using Taskify.API.DTOs.Responses.ProjectDTOs;
using Taskify.API.Enums;
using Taskify.API.Mapper;
using Taskify.API.Services.Repositories.IRepositories;
using Task = Taskify.API.Models.Task;

namespace Taskify.API.Controllers;

[Route("api/tasks")]
public class TaskController: BaseController<TaskController>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IUserProjectRepository _userProjectRepository;
    
    public TaskController(ITaskRepository taskRepository, IUserProjectRepository userProjectRepository)
    {
        _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        _userProjectRepository = userProjectRepository;
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    [Route("{projectId}/tasks")]
    public async Task<IActionResult> GetTasks(int projectId)
    {
        var tasks = await _taskRepository.GetAllTaskByProjectIdAsync(projectId);
        if (!tasks.Any())
        {
            return NotFound("No tasks found for this project.");
        }

        var response = LazyMapper.Mapper.Map<TaskResponse[]>(tasks);
        return CreateResponse(true, "Request processed successfully.", HttpStatusCode.OK, response);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin,User")]
    [Route("{projectId}/tasks")]
    public async Task<IActionResult> AddTask(int projectId, CreateTaskRequest request)
    {
        var currentUserId = GetUserIdFromToken();
        // Verify that the current user is the owner
        if (!currentUserId.HasValue)
        {
            return Unauthorized("User ID not found in token.");
        }
        var currentUserProject = await _userProjectRepository.GetByUserIdAsync(currentUserId.Value, projectId);
        if (currentUserProject == null || currentUserProject.RoleInProject != ProjectRole.Owner)
        {
            return Unauthorized("Only the project owner can add members.");
        }

        if (string.IsNullOrEmpty(request.AssignedEmail))
        {
            throw new Exception("Assigned user is required.");
        }

        var assignedUser = await _userProjectRepository.GetUserByEmailAsync(request.AssignedEmail);
        if (assignedUser == null)
        {
            return NotFound("Assigned user not found.");
        }

        var task = LazyMapper.Mapper.Map<Task>(request);
        task.ProjectId = projectId;
        task.AssignedUserId = assignedUser.Id;
        var response = await _taskRepository.AddAsync(task);
        return CreateResponse(true, "Task added successfully.", HttpStatusCode.Created, response);
    }
    
    [HttpPut]
    [Authorize(Roles = "Admin,User")]
    [Route("{projectId}/tasks/{taskId}")]
    public async Task<IActionResult> UpdateTask(int projectId, int taskId, UpdateTaskRequest request)
    {
        var currentUserId = GetUserIdFromToken();
        // Verify that the current user is the owner
        if (!currentUserId.HasValue)
        {
            return Unauthorized("User ID not found in token.");
        }
        var currentUserProject = await _userProjectRepository.GetByUserIdAsync(currentUserId.Value, projectId);
        if (currentUserProject == null || currentUserProject.RoleInProject != ProjectRole.Owner)
        {
            return Unauthorized("Only the project owner can update tasks.");
        }

        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
        {
            return NotFound("Task not found.");
        }


        LazyMapper.Mapper.Map(request, task);
        var response = await _taskRepository.UpdateAsync(task);
        return CreateResponse(true, "Task updated successfully.", HttpStatusCode.OK, response);
    }
    
    [HttpDelete]
    [Authorize(Roles = "Admin,User")]
    [Route("{projectId}/tasks/{taskId}")]
    public async Task<IActionResult> DeleteTask(int projectId, int taskId)
    {
        var currentUserId = GetUserIdFromToken();
        // Verify that the current user is the owner
        if (!currentUserId.HasValue)
        {
            return Unauthorized("User ID not found in token.");
        }
        var currentUserProject = await _userProjectRepository.GetByUserIdAsync(currentUserId.Value, projectId);
        if (currentUserProject == null || currentUserProject.RoleInProject != ProjectRole.Owner)
        {
            return Unauthorized("Only the project owner can add members.");
        }

        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
        {
            return NotFound("Task not found.");
        }

        var response = await _taskRepository.DeleteAsync(task);
        return CreateResponse(true, "Task deleted successfully.", HttpStatusCode.OK, response);
    }   
}