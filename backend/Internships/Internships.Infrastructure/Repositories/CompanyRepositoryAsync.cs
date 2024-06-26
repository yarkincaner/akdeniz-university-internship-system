using Internships.Core.Entities;
using Internships.Core.Features.Companies.Queries.GetAllCompanies;
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
    public class CompanyRepositoryAsync : GenericRepositoryAsync<Company>, ICompanyRepositoryAsync
    {
        private readonly DbSet<Company> _companies;

        public CompanyRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _companies = dbContext.Set<Company>();
        }

        public async Task<List<GetAllCompaniesViewModel>> SearchCompanies(string searchString)
        {
            var predicate = PredicateBuilder.True<Company>();
            predicate = predicate.And(company => company.IsEnabled == true);

            if (!string.IsNullOrEmpty(searchString))
            {
                var search = $"%{searchString}%";
                predicate = predicate.And(company => EF.Functions.Like(company.Name, search));
            }

            return await _companies
                .Where(predicate)
                .Select(company => new GetAllCompaniesViewModel
                {
                    Id = company.Id,
                    Name = company.Name,
                    Address = company.Address,
                    ServiceArea = company.ServiceArea,
                    Phone = company.Phone,
                    Email = company.Email,
                    Website = company.Website,
                    TaxNumber = company.TaxNumber
                })
                .ToListAsync();
        }

        public async Task<PagedResponse<IReadOnlyList<GetAllCompaniesViewModel>>> GetPagedReponseAsync(int pageNumber, int pageSize, string searchString)
        {
            var predicate = PredicateBuilder.True<Company>();
            predicate = predicate.And(company => company.IsEnabled);

            if (!string.IsNullOrEmpty(searchString))
            {
                var search = $"%{searchString}%";
                predicate = predicate.And(company => EF.Functions.Like(company.Name, search));
            }

            var filteredList = await _companies
                .Where(predicate)
                .Select(company => new GetAllCompaniesViewModel
                {
                    Id = company.Id,
                    Name = company.Name,
                    Address = company.Address,
                    ServiceArea = company.ServiceArea,
                    Phone = company.Phone,
                    Email = company.Email,
                    Website = company.Website,
                    TaxNumber = company.TaxNumber,
                    ApprovedBy = company.ApprovedBy,
                    IsApproved = company.IsApproved,
                })
                .ToListAsync();
            var totalCount = filteredList.Count;
            var pagedList = filteredList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResponse<IReadOnlyList<GetAllCompaniesViewModel>>(pagedList, pageNumber, pageSize, totalCount);
        }

        public async Task<IQueryable<Company>> GetPagedQueryableAsync(int pageNumber, int pageSize)
        {
            var queryable = _companies
                .Where(company => company.IsEnabled)
                .AsQueryable()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking();

            return await Task.FromResult(queryable);
        }

        public Task<List<Company>> GetAllCompaniesAsync()
        {
            return _companies.Where(company => company.IsEnabled).AsNoTracking().ToListAsync();
        }
    }
}
