using Internships.Core.Entities;
using Internships.Core.Exceptions;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Internships.Core.Features.Internships.Queries.GetInternshipById
{
    public class GetInternshipByIdQuery : IRequest<Response<Internship>>
    {
        public int Id { get; set; }

        public class GetInternshipByIdQueryHandler : IRequestHandler<GetInternshipByIdQuery, Response<Internship>>
        {
            private readonly IInternshipRepositoryAsync _internshipRepository;

            public GetInternshipByIdQueryHandler(IInternshipRepositoryAsync internshipRepository)
            {
                _internshipRepository = internshipRepository;
            }

            public async Task<Response<Internship>> Handle(GetInternshipByIdQuery query, CancellationToken cancellationToken)
            {
                var internship = await _internshipRepository.GetByIdAsync(query.Id);
                if (internship == null)
                {
                    throw new ApiException("Internship Not Found.");
                }
                return new Response<Internship>(internship);
            }
        }
    }
}
