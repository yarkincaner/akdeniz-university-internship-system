using Internships.Core.Entities;
using Internships.Core.Enums;
using Internships.Core.Interfaces.Repositories;
using Internships.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internships.Infrastructure.Repositories
{
    public class StatusRepositoryAsync : GenericRepositoryAsync<Status>, IStatusRepositoryAsync
    {
        private readonly DbSet<Status> _statuses;

        public StatusRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _statuses = dbContext.Set<Status>();
        }

        public async Task<IReadOnlyList<Status>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            return await _statuses
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IQueryable<Status>> GetPagedQueryableAsync(int pageNumber, int pageSize)
        {
            var queryable = _statuses
                .AsQueryable()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking();

            return await Task.FromResult(queryable);
        }

        public Task<List<Status>> GetAllStatusesAsync()
        {
            return _statuses
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Status> GetByIdAsync(int id)
        {
            return await _statuses
                .FirstOrDefaultAsync(status => status.Id == id);
        }

        public async Task<Status> GetByNameAsync(string name)
        {
            return await _statuses
                .FirstOrDefaultAsync(status => status.Name == name);
        }
    }
}
