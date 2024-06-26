using System;
using System.Collections.Generic;
using System.Text;

namespace Internships.Core.Features.Internships.Queries.GetInternshipByIdExternalService
{
    public class GetInternshipByIdExternalServiceViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public long TcKimlikNo { get; set; }
        public int BirthYear { get; set; }
        public string CompanyName { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }  

    }
}
