using Taskify.API.Models;

namespace Taskify.API.Services.Repositories.IRepositories;

public interface IUserProjectRepository : IAsyncRepository<UserProject>
{
    public Task<bool> IsEmailInProjectAsync(string email, int projectId);
    
}