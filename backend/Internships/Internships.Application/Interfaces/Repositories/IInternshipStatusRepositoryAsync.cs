using Internships.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Internships.Core.Features.InternshipStatuses.Queries.GetAllInternshipStatuses;
using Internships.Core.Features.InternshipStatuses.Queries.GetInternshipStatusesByInternshipId;

namespace Internships.Core.Interfaces.Repositories
{
    public interface IInternshipStatusRepositoryAsync : IGenericRepositoryAsync<InternshipStatus>
    {
        Task<IReadOnlyList<GetAllInternshipStatusesViewModel>> GetStatuses(int internshipId);

        Task<IReadOnlyList<GetInternshipStatusesByInternshipIdViewModel>> GetStatusesByInternshipId(int internshipId);
    }
}
