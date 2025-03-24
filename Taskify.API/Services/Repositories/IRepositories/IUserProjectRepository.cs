using Taskify.API.Models;
using Task = System.Threading.Tasks.Task;

namespace Taskify.API.Services.Repositories.IRepositories;

public interface IUserProjectRepository : IAsyncRepository<UserProject>
{
   
    public Task<UserProject?> GetByUserIdAsync(int userId, int projectId);

    Task<IEnumerable<UserProject>> GetAllMembersByProjectIdAsync(int projectId);
    Task<UserProject> AddAsync(UserProject entity, IEnumerable<int> userIds);
    Task<IEnumerable<int>> GetUserIdsByEmailsAsync(IEnumerable<string> requestMemberEmails);
    Task<bool> RemoveMemberFromProjectByEmailAsync(string email, int projectId);
    Task<User?> GetUserByEmailAsync(string email);
}