using System.Collections.Generic;

namespace Internships.Core.Entities
{
    public class Department : AuditableBaseEntity
    {
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
        public bool IsEnabled { get; set; }
    }
}