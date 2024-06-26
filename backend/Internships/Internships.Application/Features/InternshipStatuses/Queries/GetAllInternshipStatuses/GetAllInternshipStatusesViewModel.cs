using Internships.Core.Enums;

namespace Internships.Core.Features.InternshipStatuses.Queries.GetAllInternshipStatuses
{
    public class GetAllInternshipStatusesViewModel
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public int InternshipId { get; set; }
        public string InternshipUserName { get; set; }

        public string Comment { get; set; }
    }
}
