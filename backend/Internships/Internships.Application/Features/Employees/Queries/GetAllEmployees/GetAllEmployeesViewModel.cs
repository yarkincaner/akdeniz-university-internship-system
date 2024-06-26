using Internships.Core.Entities;

namespace Internships.Core.Features.Employees.Queries.GetAllEmployees
{
    public class GetAllEmployeesViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}
