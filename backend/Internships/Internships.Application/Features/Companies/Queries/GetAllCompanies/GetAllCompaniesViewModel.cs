namespace Internships.Core.Features.Companies.Queries.GetAllCompanies
{
    public class GetAllCompaniesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ServiceArea { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string TaxNumber { get; set; }
        public string ApprovedBy { get; set; }
        public bool IsApproved { get; set; }

    }
}
