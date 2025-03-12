using Taskify.API.Models;

namespace Taskify.API.Services.Repositories.IRepositories
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        public Task<User> UserLogin(string email, string password);
        public Task<User> GetUserByEmailAsync(string email);
    }
}
