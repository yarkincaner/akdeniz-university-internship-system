using Internships.Core.Enums;
using System;

namespace Internships.Core.Parameters
{
    public class CreateInternshipCommandParameter
    {
        public int CompanyId { get; set; }

        public int EmployeeId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public InsuranceType InsuranceType { get; set; }
    }
}
