using Internships.Core.Entities;
using Internships.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internships.Core.Interfaces.Repositories
{
    public interface IStatusRepositoryAsync : IGenericRepositoryAsync<Status>
    {
        Task<List<Status>> GetAllStatusesAsync();
        Task<Status> GetByNameAsync(string name);
    }
}
