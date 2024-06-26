using System.Collections.Generic;

namespace Internships.Core.Entities
{
    public class Employee : AuditableBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        
        public virtual ICollection<Internship> Internships { get; set; }
        public bool IsEnabled { get; set; }
    }
}