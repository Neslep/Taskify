using Taskify.API.Models;

namespace Taskify.API.Services.Repositories.IRepositories
{
    public interface IAdminRepository : IAsyncRepository<Admin>
    {
        public Task<Admin> AdminLogin(string userName, string password);
    }
}
