using Internships.Core.Entities;
using Internships.Core.Exceptions;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Companies.Queries.GetCompanyById
{
    public class GetCompanyByIdQuery : IRequest<Response<Company>>
    {
        public int Id { get; set; }

        public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, Response<Company>>
        {
            private readonly ICompanyRepositoryAsync _companyRepository;

            public GetCompanyByIdQueryHandler(ICompanyRepositoryAsync companyRepository)
            {
                _companyRepository = companyRepository;
            }

            public async Task<Response<Company>> Handle(GetCompanyByIdQuery query, CancellationToken cancellationToken)
            {
                var company = await _companyRepository.GetByIdAsync(query.Id);
                if (company == null)
                {
                    throw new EntityNotFoundException($"Company with id {query.Id} not found");
                }
                return new Response<Company>(company);
            }
        }
    }
}
