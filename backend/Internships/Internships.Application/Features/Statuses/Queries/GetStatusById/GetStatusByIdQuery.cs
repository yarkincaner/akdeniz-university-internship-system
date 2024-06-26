using Internships.Core.Entities;
using Internships.Core.Exceptions;
using Internships.Core.Interfaces.Repositories;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Internships.Core.Wrappers;

namespace Internships.Core.Features.Statuses.Queries.GetStatusById
{
    public class GetStatusByIdQuery : IRequest<Response<Status>>
    {
        public int Id { get; set; }

        public class GetStatusByIdQueryHandler : IRequestHandler<GetStatusByIdQuery, Response<Status>>
        {
            private readonly IStatusRepositoryAsync _statusRepository;

            public GetStatusByIdQueryHandler(IStatusRepositoryAsync statusRepository)
            {
                _statusRepository = statusRepository;
            }

            public async Task<Response<Status>> Handle(GetStatusByIdQuery query, CancellationToken cancellationToken)
            {
                var status = await _statusRepository.GetByIdAsync(query.Id);
                if (status == null)
                {
                    throw new EntityNotFoundException("Status", query.Id);
                }
                return new Response<Status>(status);
            }
        }
    }
}
