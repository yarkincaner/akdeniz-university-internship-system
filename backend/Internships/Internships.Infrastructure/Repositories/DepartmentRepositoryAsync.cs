using Internships.Core.Entities;
using Internships.Core.Interfaces.Repositories;
using Internships.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internships.Infrastructure.Repositories
{
    public class DepartmentRepositoryAsync : GenericRepositoryAsync<Department>, IDepartmentRepositoryAsync
    {
        private readonly DbSet<Department> _departments;

        public DepartmentRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _departments = dbContext.Set<Department>();
        }

        public async Task<IReadOnlyList<Department>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            return await _departments
                .Where(department => department.IsEnabled)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IQueryable<Department>> GetPagedQueryableAsync(int pageNumber, int pageSize)
        {
            var queryable = _departments
                .Where(department => department.IsEnabled)
                .AsQueryable()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking();

            return await Task.FromResult(queryable);
        }

        public Task<List<Department>> GetAllDepartmentsAsync()
        {
            return _departments
                .Where(department => department.IsEnabled)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
