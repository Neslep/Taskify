using System.Linq.Expressions;

namespace Taskify.API.Services.Repositories.IRepositories
{
    public interface IAsyncRepository<T>
    {
        public Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        public Task<T?> GetByIdAsync(int id);
        public Task<T> AddAsync(T entity);
        public Task<bool> UpdateAsync(T entity);
        public Task<bool> DeleteAsync(T entity);
    }
}
