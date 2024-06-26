using Internships.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internships.Core.Interfaces.Repositories
{
    public interface IDepartmentRepositoryAsync : IGenericRepositoryAsync<Department>
    {
        Task<List<Department>> GetAllDepartmentsAsync();
    }
}
