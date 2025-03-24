namespace Taskify.API.Services.Repositories.IRepositories;
public interface ITaskRepository : IAsyncRepository<Models.Task>
{
    Task<IEnumerable<Models.Task>> GetAllTaskByProjectIdAsync(int projectId);
    
}