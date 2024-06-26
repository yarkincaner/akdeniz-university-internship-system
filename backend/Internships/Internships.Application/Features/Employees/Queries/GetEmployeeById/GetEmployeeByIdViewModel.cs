using Internships.Core.Features.Internships.Queries.GetAllInternships;
using System.Collections.Generic;

namespace Internships.Core.Features.Employees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public ICollection<GetAllInternshipsViewModel> Internships { get; set; }
    }
}
