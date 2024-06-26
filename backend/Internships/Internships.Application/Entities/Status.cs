using Internships.Core.Enums;
using System.Collections.Generic;

namespace Internships.Core.Entities
{
    public class Status : BaseEntity
    {
        public Status() 
        {
            InternshipStatuses = new List<InternshipStatus>();
        }
        public string Name {  get; set; }
        public ICollection<InternshipStatus> InternshipStatuses { get; set; }
    }
}