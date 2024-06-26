using Internships.Core.Parameters;

namespace Internships.Core.Features.Employees.Queries.GetAllEmployees
{
    public class GetAllEmployeesParameter : RequestParameter
    {
        public string SearchString { get; set; }
    }
}
