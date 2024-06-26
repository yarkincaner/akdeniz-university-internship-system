using Internships.Core.Parameters;

namespace Internships.Core.Features.Internships.Queries.GetAllInternships
{
    public class GetAllInternshipsParameter : RequestParameter
    {
        public string SearchString { get; set; }
    }
}
