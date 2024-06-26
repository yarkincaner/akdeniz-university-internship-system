using Internships.Core.Entities;
using Internships.Core.Features.Employees.Queries.GetAllEmployees;
using Internships.Core.Features.Employees.Queries.GetEmployeeById;
using Internships.Core.Wrappers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internships.Core.Interfaces.Repositories
{
    public interface IEmployeeRepositoryAsync : IGenericRepositoryAsync<Employee>
    {
        Task<PagedResponse<IReadOnlyList<GetAllEmployeesViewModel>>> GetPagedResponseAsync(int pageNumber, int pageSize, string searchString);
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<List<GetAllEmployeesViewModel>> SearchEmployees(string searchString);
        Task<List<GetAllEmployeesViewModel>> SearchEmployeesByCompanyId(int companyId, string searchString);
        Task<PagedResponse<IReadOnlyList<GetAllEmployeesViewModel>>> GetPagedResponseByCompanyIdAsync(int companyId, int pageNumber, int pageSize, string searchString);
        Task<GetEmployeeByIdViewModel> GetEmployeeById(int id);
    }
}
