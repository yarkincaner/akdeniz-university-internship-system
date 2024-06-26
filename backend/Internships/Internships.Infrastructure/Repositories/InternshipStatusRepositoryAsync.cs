using Internships.Core.Entities;
using Internships.Core.Interfaces.Repositories;
using Internships.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internships.Core.Features.InternshipStatuses.Queries.GetAllInternshipStatuses;
using Internships.Core.Features.InternshipStatuses.Queries.GetInternshipStatusesByInternshipId;

namespace Internships.Infrastructure.Repositories
{
    public class InternshipStatusRepositoryAsync : GenericRepositoryAsync<InternshipStatus>,
        IInternshipStatusRepositoryAsync
    {
        private readonly DbSet<InternshipStatus> _internshipStatuses;

        public InternshipStatusRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _internshipStatuses = dbContext.Set<InternshipStatus>();
        }

        public async Task<IReadOnlyList<GetAllInternshipStatusesViewModel>> GetStatuses(int internshipId)
        {
            return await _internshipStatuses
                .Where(i => i.InternshipId == internshipId)
                .Select(i => new GetAllInternshipStatusesViewModel
                {
                    InternshipId = i.InternshipId,
                    StatusId = i.StatusId,
                    StatusName = i.Status.Name,
                    Comment = i.Comment
                })
                .ToListAsync();
        }

        public async Task<IQueryable<InternshipStatus>> GetPagedQueryableAsync(int pageNumber, int pageSize)
        {
            var queryable = _internshipStatuses
                .Include(i => i.Internship)
                .Include(i => i.Status)
                .AsQueryable()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking();

            return await Task.FromResult(queryable);
        }

        public Task<List<InternshipStatus>> GetAllInternshipStatusesAsync()
        {
            return _internshipStatuses
                .Include(i => i.Internship)
                .Include(i => i.Status)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<InternshipStatus> GetByIdAsync(int id)
        {
            return await _internshipStatuses
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<GetInternshipStatusesByInternshipIdViewModel>> GetStatusesByInternshipId(int internshipId)
        {
            return await _internshipStatuses
                .Include(i => i.Internship)
                .Include(i => i.Status)
                .Where(i => i.InternshipId == internshipId)
                .Select(i => new GetInternshipStatusesByInternshipIdViewModel
                {
                    Id = i.Id,
                    InternshipId = i.InternshipId,
                    StatusId = i.StatusId,
                    StatusName = i.Status.Name,
                    Comment = i.Comment,
                    Created = i.Created
                })
                .ToListAsync();
        }
    }
}