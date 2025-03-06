using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Taskify.API.Data;
using Taskify.API.Exceptions;
using Taskify.API.Models;
using Taskify.API.Services.Repositories.IRepositories;

namespace Taskify.API.Services.Repositories.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(TaskifyContext dbContext) : base(dbContext)
        {
        }

        public Task<User> AddAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<User>> GetAllAsync(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var user = await _dbContext.Users.Where(o => o.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException("Not found any users with id: " + id);
            }
            return user;
        }

        public Task<bool> UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public async Task<User> UserLogin(string email, string password)
        {
            string hashedPassword = HashPassword(password);

            var user = await _dbContext.Users.Where(o => o.Email.Equals(email)
                && o.PasswordHash.Equals(hashedPassword)).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new BadRequestException("Wrong email or password");
            }
            if (user.Status != Enums.UserStatus.Active)
            {
                throw new BadRequestException("User account is " + user.Status.ToString());
            }
            return user;
        }
    }
}
