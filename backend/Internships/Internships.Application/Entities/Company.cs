using System.Collections.Generic;

namespace Internships.Core.Entities
{
    public class Company : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ServiceArea { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string TaxNumber { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovedBy { get; set; }
        public ICollection<Employee> Employees { get; set; }       
        public ICollection<Internship> Internships { get; set; }   
    }
}