using Internships.Core.Entities;
using Internships.Core.Exceptions;
using Internships.Core.Interfaces.Repositories;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Internships.Core.Wrappers;

namespace Internships.Core.Features.InternshipStatuses.Queries.GetInternshipStatusById
{
    public class GetInternshipStatusByIdQuery : IRequest<Response<InternshipStatus>>
    {
        public int Id { get; set; }

        public class GetInternshipStatusByIdQueryHandler : IRequestHandler<GetInternshipStatusByIdQuery, Response<InternshipStatus>>
        {
            private readonly IInternshipStatusRepositoryAsync _internshipStatusRepository;

            public GetInternshipStatusByIdQueryHandler (IInternshipStatusRepositoryAsync internshipStatusRepository)
            {
                _internshipStatusRepository = internshipStatusRepository;
            }

            public async Task<Response<InternshipStatus>> Handle(GetInternshipStatusByIdQuery query, CancellationToken cancellationToken)
            {
                var internshipStatus = await _internshipStatusRepository.GetByIdAsync(query.Id);
                if (internshipStatus == null)
                {
                    throw new EntityNotFoundException("InternshipStatus", query.Id);
                }
                return new Response<InternshipStatus>(internshipStatus);
            }
        }
    }
}
