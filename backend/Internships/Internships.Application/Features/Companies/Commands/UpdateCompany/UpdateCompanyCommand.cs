using Internships.Core.Exceptions;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Companies.Commands.UpdateCompany
{
    public class UpdateCompanyCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ServiceArea { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string TaxNumber { get; set; }

        public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Response<int>>
        {
            private readonly ICompanyRepositoryAsync _companyRepository;

            public UpdateCompanyCommandHandler(ICompanyRepositoryAsync companyRepository)
            {
                _companyRepository = companyRepository;
            }

            public async Task<Response<int>> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken)
            {
                var company = await _companyRepository.GetByIdAsync(command.Id);

                if (company == null)
                {
                    throw new EntityNotFoundException("company", command.Id);
                }

                company.Name = command.Name;
                company.Address = command.Address;
                company.ServiceArea = command.ServiceArea;
                company.Phone = command.Phone;
                company.Email = command.Email;
                company.Website = command.Website;
                company.TaxNumber = command.TaxNumber;

                await _companyRepository.UpdateAsync(company);
                return new Response<int>(company.Id);
            }
        }
    }
}
