using Internships.Core.Exceptions;
using Internships.Core.Interfaces;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Companies.Commands.ApproveCompanyById
{
    public class ApproveCompanyByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public bool IsApproved { get; set; }
        public class ApproveCompanyByIdCommandHandler : IRequestHandler<ApproveCompanyByIdCommand, Response<int>>
        {
            private readonly ICompanyRepositoryAsync _companyRepository;
            private readonly IAuthenticatedUserService _authenticatedUser;

            public ApproveCompanyByIdCommandHandler(ICompanyRepositoryAsync companyRepository, IAuthenticatedUserService authenticatedUser)
            {
                _companyRepository = companyRepository;
                _authenticatedUser = authenticatedUser;
            }


            public async Task<Response<int>> Handle(ApproveCompanyByIdCommand command, CancellationToken cancellationToken)
            {
                var company=await _companyRepository.GetByIdAsync(command.Id);
                if(company==null)
                { 
                        throw new EntityNotFoundException("Company",command.Id);
                }

                company.ApprovedBy = _authenticatedUser.UserId;
                company.IsApproved = command.IsApproved;

                await _companyRepository.UpdateAsync(company);
                return new Response<int>(company.Id);
            }
        }
    }
    

}
