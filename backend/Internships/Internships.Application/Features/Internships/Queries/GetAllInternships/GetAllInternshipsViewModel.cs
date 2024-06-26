﻿using Internships.Core.Entities;
using Internships.Core.Enums;
using System;
using System.Collections.Generic;

namespace Internships.Core.Features.Internships.Queries.GetAllInternships
{
    public class GetAllInternshipsViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public long UserTcKimlikNo { get; set; }

        public string StatusName { get; set; }
        public string Comment { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public InsuranceType InsuranceType { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovedBy { get; set; }
    }
}
