using Internships.Core.Entities;
using Internships.Core.Features.Employees.Queries.GetAllEmployees;
using Internships.Core.Features.Employees.Queries.GetEmployeeById;
using Internships.Core.Features.Internships.Queries.GetAllInternships;
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
    public class EmployeeRepositoryAsync : GenericRepositoryAsync<Employee>, IEmployeeRepositoryAsync
    {
        private readonly DbSet<Employee> _employees;

        public EmployeeRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _employees = dbContext.Set<Employee>();
        }

        public async Task<PagedResponse<IReadOnlyList<GetAllEmployeesViewModel>>> GetPagedResponseAsync(int pageNumber, int pageSize, string searchString)
        {
            var predicate = PredicateBuilder.True<Employee>();
            predicate = predicate.And(employee => employee.IsEnabled);

            if (!string.IsNullOrEmpty(searchString))
            {
                var search = $"%{searchString}%";
                predicate = predicate.And(employee => EF.Functions.Like(employee.FirstName, search) || EF.Functions.Like(employee.LastName, search));
            }

            var filteredList = await _employees
                .Where(predicate)
                .Select(employee => new GetAllEmployeesViewModel
                {
                    Id = employee.Id,
                    CompanyId = employee.CompanyId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    CompanyName = employee.Company.Name,
                    Title = employee.Title
                })
                .ToListAsync();
            var totalCount = filteredList.Count;
            var pagedList = filteredList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResponse<IReadOnlyList<GetAllEmployeesViewModel>>(pagedList, pageNumber, pageSize, totalCount);
        }

        public async Task<IQueryable<Employee>> GetPagedQueryableAsync(int pageNumber, int pageSize)
        {
            var queryable = _employees
                .Where(employee => employee.IsEnabled)
                .AsQueryable()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking();

            return await Task.FromResult(queryable);
        }

        public Task<List<Employee>> GetAllEmployeesAsync()
        {
            return _employees
                .Where(employee => employee.IsEnabled)
                .Include(i => i.Company)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<GetAllEmployeesViewModel>> SearchEmployees(string searchString)
        {
            var predicate = PredicateBuilder.True<Employee>();
            predicate = predicate.And(employee => employee.IsEnabled);

            if (!string.IsNullOrEmpty(searchString))
            {
                var search = $"%{searchString}%";
                predicate = predicate.And(employee => EF.Functions.Like(employee.FirstName, search) || EF.Functions.Like(employee.LastName, search));
            }

            return await _employees
                .Where(predicate)
                .Select(employee => new GetAllEmployeesViewModel
                {
                    Id = employee.Id,
                    CompanyId = employee.CompanyId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    CompanyName = employee.Company.Name,
                    Title = employee.Title
                })
                .ToListAsync();
        }

        public async Task<List<GetAllEmployeesViewModel>> SearchEmployeesByCompanyId(int companyId, string searchString)
        {
            var predicate = PredicateBuilder.True<Employee>();
            predicate = predicate.And(employee => employee.IsEnabled && employee.CompanyId == companyId);

            if (!string.IsNullOrEmpty(searchString))
            {
                var search = $"%{searchString}%";
                predicate = predicate.And(employee => EF.Functions.Like(employee.FirstName, search) || EF.Functions.Like(employee.LastName, search));
            }

            return await _employees
                .Where(predicate)
                .Select(employee => new GetAllEmployeesViewModel
                {
                    Id = employee.Id,
                    CompanyId = employee.CompanyId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    CompanyName = employee.Company.Name,
                    Title = employee.Title
                })
                .ToListAsync();
        }

        public async Task<PagedResponse<IReadOnlyList<GetAllEmployeesViewModel>>> GetPagedResponseByCompanyIdAsync(int companyId, int pageNumber, int pageSize, string searchString)
        {
            var predicate = PredicateBuilder.True<Employee>();
            predicate = predicate.And(employee => employee.IsEnabled);
            predicate = predicate.And(employee => employee.CompanyId == companyId);

            if (!string.IsNullOrEmpty(searchString))
            {
                var search = $"%{searchString}%";
                predicate = predicate.And(employee => EF.Functions.Like(employee.FirstName, search) || EF.Functions.Like(employee.LastName, search));
            }

            var filteredList = await _employees
                .Where(predicate)
                .Select(employee => new GetAllEmployeesViewModel
                {
                    Id = employee.Id,
                    CompanyId = employee.CompanyId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    CompanyName = employee.Company.Name,
                    Title = employee.Title
                })
                .ToListAsync();
            var totalCount = filteredList.Count;
            var pagedList = filteredList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResponse<IReadOnlyList<GetAllEmployeesViewModel>>(pagedList, pageNumber, pageSize, totalCount);
        }

        public async Task<GetEmployeeByIdViewModel> GetEmployeeById(int id)
        {
            var predicate = PredicateBuilder.True<Employee>();
            predicate = predicate.And(employee => employee.Id == id);
            predicate = predicate.And(employee => employee.IsEnabled);

            return await _employees
                .Where(predicate)
                .Select(employee => new GetEmployeeByIdViewModel
                {
                    Id = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Title = employee.Title,
                    Email = employee.Email,
                    CompanyId = employee.CompanyId,
                    CompanyName = employee.Company.Name,
                    Internships = employee.Internships
                        .Select(internship => new GetAllInternshipsViewModel
                        {
                            Id = internship.Id,
                            UserEmail = internship.User.Email,
                            UserName = internship.User.FirstName + " " + internship.User.LastName,
                            StartDate = internship.StartDate,
                            EndDate = internship.EndDate,
                            TotalDays = internship.TotalDays,
                            InsuranceType = internship.InsuranceType,
                            StatusName = internship.InternshipStatuses.OrderByDescending(s => s.Created).FirstOrDefault().Status.Name,
                            Comment = internship.InternshipStatuses.OrderByDescending(s => s.Created).FirstOrDefault().Comment,
                        }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}
