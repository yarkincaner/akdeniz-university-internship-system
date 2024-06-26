using System;
using System.Collections.Generic;
using System.Text;

namespace Internships.Core.Features.InternshipStatuses.Queries.GetInternshipStatusesByInternshipId
{
    public class GetInternshipStatusesByInternshipIdViewModel
    {
        public int Id { get; set; }
        public int InternshipId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
    }
}
