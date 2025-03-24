using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Taskify.API.Data;
using Taskify.API.Services.Repositories.IRepositories;
using Task = Taskify.API.Models.Task;

namespace Taskify.API.Services.Repositories.Repositories;

public class TaskRepository : RepositoryBase, ITaskRepository
{
    private new readonly TaskifyContext _dbContext;

    public TaskRepository(TaskifyContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Task>> GetAllAsync(Expression<Func<Task, bool>> predicate)
    {
        return await _dbContext.Tasks.Where(predicate).ToListAsync();
    }

    public async Task<Task?> GetByIdAsync(int id)
    {
        return await _dbContext.Tasks.FindAsync(id);
    }

    public async Task<Task> AddAsync(Task entity)
    {
        entity.LastModifiedDate = DateTime.UtcNow;
        entity.CreatedDate = DateTime.UtcNow;

        await _dbContext.Tasks.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Task entity)
    {
        entity.LastModifiedDate = DateTime.UtcNow;
        _dbContext.Tasks.Update(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(Task entity)
    {
        _dbContext.Tasks.Remove(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<Task>> GetAllTaskByProjectIdAsync(int projectId)
    {
        return await _dbContext.Tasks
            .Where(t => t.ProjectId == projectId)
            .Include(up => up.AssignedUser) // Ensure this is a navigation property
            .ToListAsync(); // Ensure not to return null
    }
}