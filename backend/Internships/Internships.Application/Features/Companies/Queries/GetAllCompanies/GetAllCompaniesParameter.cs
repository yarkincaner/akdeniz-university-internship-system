using Internships.Core.Parameters;

namespace Internships.Core.Features.Companies.Queries.GetAllCompanies
{
    public class GetAllCompaniesParameter : RequestParameter
    {
        public string SearchString { get; set; }
    }
}
