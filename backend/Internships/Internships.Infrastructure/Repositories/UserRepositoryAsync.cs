using Internships.Core.Entities;
using Internships.Core.Interfaces.Repositories;
using Internships.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internships.Infrastructure.Repositories
{
    public class UserRepositoryAsync : GenericRepositoryAsync<User>, IUserRepositoryAsync
    {
        private readonly DbSet<User> _users;

        public UserRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _users = dbContext.Set<User>();
        }

        public async Task<IReadOnlyList<User>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            return await _users
                .Where(user => user.IsEnabled)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IQueryable<User>> GetPagedQueryableAsync(int pageNumber, int pageSize)
        {
            var queryable = _users
                .Where(user => user.IsEnabled)
                .AsQueryable()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking();

            return await Task.FromResult(queryable);
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            return _users
                .Where(user => user.IsEnabled)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task<User> GetByUserId(string userId)
        {
            return _users.FirstAsync(user => user.UserId == userId);
        }
    }
}
