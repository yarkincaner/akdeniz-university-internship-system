using Internships.Core.Entities;
using Internships.Core.Features.Companies.Queries.GetAllCompanies;
using Internships.Core.Wrappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internships.Core.Interfaces.Repositories
{
    public interface ICompanyRepositoryAsync : IGenericRepositoryAsync<Company>
    {
        Task<PagedResponse<IReadOnlyList<GetAllCompaniesViewModel>>> GetPagedReponseAsync(int pageNumber, int pageSize, string searchString);
        Task<List<GetAllCompaniesViewModel>> SearchCompanies(string searchString);
    }
}
