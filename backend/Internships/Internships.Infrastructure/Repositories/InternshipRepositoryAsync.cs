using Internships.Core.Entities;
using Internships.Core.Exceptions;
using Internships.Core.Features.Internships.Queries.GetAllInternships;
using Internships.Core.Features.Internships.Queries.GetInternshipByIdExternalService;
using Internships.Core.Features.Internships.Queries.GetInternshipsByUserId;
using Internships.Core.Helpers;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using Internships.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internships.Infrastructure.Repositories
{
    public class InternshipRepositoryAsync : GenericRepositoryAsync<Internship>, IInternshipRepositoryAsync
    {
        private readonly DbSet<Internship> _internships;

        public InternshipRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _internships = dbContext.Set<Internship>();
        }

        public async Task<PagedResponse<IReadOnlyList<GetAllInternshipsViewModel>>> GetPagedResponseAsync(int pageNumber, int pageSize, string searchString)
        {
            var predicate = PredicateBuilder.True<Internship>();
            predicate = predicate.And(internship => internship.IsEnabled);

            if (!string.IsNullOrEmpty(searchString))
            {
                var search = $"%{searchString}%";
                predicate = predicate.And(internship => EF.Functions.Like(internship.Company.Name, search) || EF.Functions.Like(internship.Employee.FirstName, search) || EF.Functions.Like(internship.Employee.LastName, search) || EF.Functions.Like(internship.User.Email, search) || EF.Functions.Like(internship.User.FirstName, search) || EF.Functions.Like(internship.User.LastName, search));
            }

            var filteredList = await _internships
                .Where(predicate)
                .Select(internship => new GetAllInternshipsViewModel
                {
                    Id = internship.Id,
                    UserEmail = internship.User.Email,
                    UserName = internship.User.FirstName + " " + internship.User.LastName,
                    UserTcKimlikNo = internship.User.TcKimlikNo,
                    CompanyId = internship.CompanyId,
                    CompanyName = internship.Company.Name,
                    EmployeeId = internship.Employee.Id,
                    EmployeeName = internship.Employee.FirstName + " " + internship.Employee.LastName,
                    StartDate = internship.StartDate,
                    EndDate = internship.EndDate,
                    TotalDays = internship.TotalDays,
                    InsuranceType = internship.InsuranceType,
                    StatusName = internship.InternshipStatuses.OrderByDescending(s => s.Created).FirstOrDefault().Status.Name,
                })
                .ToListAsync();
            var totalCount = filteredList.Count;
            var pagedList = filteredList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResponse<IReadOnlyList<GetAllInternshipsViewModel>>(pagedList, pageNumber, pageSize, totalCount);
        }

        public async Task<IQueryable<Internship>> GetPagedQueryableAsync(int pageNumber, int pageSize)
        {
            var queryable = _internships
                .Where(internship => internship.IsEnabled)
                .AsQueryable()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking();

            return await Task.FromResult(queryable);
        }

        public async Task<List<GetAllInternshipsViewModel>> GetAllInternshipsAsync()
        {
            var predicate = PredicateBuilder.True<Internship>();
            predicate = predicate.And(internship => internship.IsEnabled);

            return await _internships
                .Where(predicate)
                .Select(internship => new GetAllInternshipsViewModel
                {
                    Id = internship.Id,
                    CompanyId = internship.CompanyId,
                    CompanyName = internship.Company.Name,
                    EmployeeId = internship.EmployeeId,
                    EmployeeName = internship.Employee.FirstName + " " + internship.Employee.LastName,
                    UserId = internship.UserId,
                    UserName = internship.User.FirstName + " " + internship.User.LastName,
                    UserEmail = internship.User.Email,
                    StartDate = internship.StartDate,
                    EndDate = internship.EndDate,
                    TotalDays = internship.TotalDays,
                    InsuranceType = internship.InsuranceType,
                    Comment = internship.InternshipStatuses.OrderByDescending(s => s.Created).FirstOrDefault().Comment,
                    StatusName = internship.InternshipStatuses.OrderByDescending(s => s.Created).FirstOrDefault().Status.Name,
                    Documents = internship.Documents,
                    ApprovedBy = internship.ApprovedBy,
                    IsApproved = internship.IsApproved,
                })
                .ToListAsync();
        }

        public async Task<Internship> GetByIdAsync(int id)
        {
            return await _internships
                .Where(internship => internship.IsEnabled && internship.Id == id)
                .Include(i => i.Company)
                .Include(i => i.Employee)
                .Include(i => i.User)
                .Include(i => i.InternshipStatuses)
                .ThenInclude(i => i.Status)
                .FirstOrDefaultAsync();
        }

        public async Task<List<GetInternshipsByUserIdViewModel>> GetInternshipByUserIdAsync(int pageNumber, int pageSize, string UserId)
        {
            var predicate = PredicateBuilder.True<Internship>();
            predicate = predicate.And(internship => internship.IsEnabled && internship.UserId == UserId);

            return await _internships
                .Where(predicate)
                .Select(internship => new GetInternshipsByUserIdViewModel
                {
                    Id = internship.Id,
                    CompanyId = internship.CompanyId,
                    CompanyName = internship.Company.Name,
                    EmployeeId = internship.EmployeeId,
                    EmployeeName = internship.Employee.FirstName + " " + internship.Employee.LastName,
                    StartDate = internship.StartDate,
                    EndDate = internship.EndDate,
                    TotalDays = internship.TotalDays,
                    InsuranceType = internship.InsuranceType,
                    Comment = internship.InternshipStatuses.OrderByDescending(s => s.Created).FirstOrDefault().Comment,
                    StatusName = internship.InternshipStatuses.OrderByDescending(s => s.Created).FirstOrDefault().Status.Name,
                    Documents = internship.Documents,
                    ApprovedBy = internship.ApprovedBy,
                    IsApproved = internship.IsApproved,
                })
                .ToListAsync();
        }

        public async Task<List<Internship>> GetAllInternshipsByStatusAsync(string name)
        {
            return await _internships
                .Where(internship => internship.IsEnabled && internship.InternshipStatuses.OrderByDescending(s => s.Created).FirstOrDefault().Status.Name.Equals(name))
                .Include(i => i.Company)
                .Include(i => i.Employee)
                .Include(i => i.User)
                .Include(i => i.InternshipStatuses)
                .ThenInclude(i => i.Status)
                .ToListAsync();
        }

        public async Task<Response<GetInternshipByIdExternalServiceViewModel>> GetInternshipByIdExternalServiceAsync(int id)
        {
            var internship = await _internships.Include(p => p.User).Include(p => p.Employee).Include(p => p.Company).FirstOrDefaultAsync(p => p.Id == id);
            if (internship == null)
            {
                throw new EntityNotFoundException(id);
            }
            var result = new GetInternshipByIdExternalServiceViewModel
            {
                Id = internship.Id,
                FirstName = internship.User.FirstName,
                LastName = internship.User.LastName,
                BirthYear = internship.User.BirthYear,
                TcKimlikNo = internship.User.TcKimlikNo,
                Email = internship.User.Email,
                CompanyName = internship.Company.Name,
                EmployeeName = internship.Employee.FirstName + " " + internship.Employee.LastName,
                StartDate = internship.StartDate,
                EndDate = internship.EndDate,
            };
            return new Response<GetInternshipByIdExternalServiceViewModel>(result);
        }

        public async Task<Internship> GetInternshipsWithStatus(int internshipId)
        {
            return await _internships.Where(i => i.Id == internshipId).Include(i => i.InternshipStatuses).FirstOrDefaultAsync();
        }
    }
}
