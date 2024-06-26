using Internships.Core.Enums;
using System;
using System.Collections.Generic;

namespace Internships.Core.Entities
{
    public class Internship : AuditableBaseEntity
    {
        public Internship()
        {
            InternshipStatuses = new List<InternshipStatus>();
            Documents = new List<Document>();
            IsEnabled = true;
        }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public ICollection<InternshipStatus> InternshipStatuses { get; set; }
        public ICollection<Document> Documents { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public InsuranceType InsuranceType { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovedBy { get; set; }
    }
}