using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Taskify.API.Data;

namespace Taskify.API.Services.Repositories.Repositories
{
    public class RepositoryBase
    {
        protected readonly TaskifyContext _dbContext;

        public RepositoryBase(TaskifyContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        protected string HashPassword(string? password)
        {
            byte[] salt = [2, 2, 4, 4, 4, 6, 6, 4];
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
