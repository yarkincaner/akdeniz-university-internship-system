using Internships.Core.Entities;
using Internships.Core.Features.Internships.Queries.GetAllInternships;
using Internships.Core.Features.Internships.Queries.GetInternshipByIdExternalService;
using Internships.Core.Features.Internships.Queries.GetInternshipsByUserId;
using Internships.Core.Wrappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internships.Core.Interfaces.Repositories
{
    public interface IInternshipRepositoryAsync : IGenericRepositoryAsync<Internship>
    {
        Task<PagedResponse<IReadOnlyList<GetAllInternshipsViewModel>>> GetPagedResponseAsync(int pageNumber, int pageSize, string searchString);
        Task<List<GetAllInternshipsViewModel>> GetAllInternshipsAsync();

        Task<List<GetInternshipsByUserIdViewModel>> GetInternshipByUserIdAsync(int pageNumber, int pageSize, string UserId);

        Task<List<Internship>> GetAllInternshipsByStatusAsync(string name);

        Task<Response<GetInternshipByIdExternalServiceViewModel>> GetInternshipByIdExternalServiceAsync(int id);
        Task<Internship> GetInternshipsWithStatus(int internshipId);

    }
}
