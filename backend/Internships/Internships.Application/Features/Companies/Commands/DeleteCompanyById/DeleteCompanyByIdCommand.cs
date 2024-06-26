using Internships.Core.Exceptions;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Companies.Commands.DeleteCompanyById
{
    public class DeleteCompanyByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }

        public class DeleteCompanyByIdCommandHandler : IRequestHandler<DeleteCompanyByIdCommand, Response<int>>
        {
            private readonly ICompanyRepositoryAsync _companyRepository;

            public DeleteCompanyByIdCommandHandler(ICompanyRepositoryAsync companyRepository)
            {
                _companyRepository = companyRepository;
            }

            public async Task<Response<int>> Handle(DeleteCompanyByIdCommand command, CancellationToken cancellationToken)
            {
                var company = await _companyRepository.GetByIdAsync(command.Id);
                if (company == null) throw new EntityNotFoundException($"The company with id {command.Id} was not found");
                company.IsEnabled = false;
                await _companyRepository.UpdateAsync(company);
                return new Response<int>(company.Id);
            }
        }
    }
}
