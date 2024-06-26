using Internships.Core.Entities;
using Internships.Core.Interfaces.Repositories;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Internships.Core.Wrappers;
using System.Collections.Generic;
using Internships.Core.Enums;

namespace Internships.Core.Features.Statuses.Queries.GetStatusByName
{
    public class GetStatusByNameQuery : IRequest<Response<Status>>
    {
        public string Name { get; set; }

        public class GetStatusByNameQueryHandler : IRequestHandler<GetStatusByNameQuery, Response<Status>>
        {
            private readonly IStatusRepositoryAsync _statusRepository;

            public GetStatusByNameQueryHandler(IStatusRepositoryAsync statusRepository)
            {
                _statusRepository = statusRepository;
            }

            public async Task<Response<Status>> Handle(GetStatusByNameQuery query, CancellationToken cancellationToken)
            {
                //TODO: Modify the query to return the status with the given name
                var status = await _statusRepository.GetByNameAsync(query.Name);
                if (status == null)
                {
                    status = new Status
                    {
                        Name = "Pending",
                        InternshipStatuses = new List<InternshipStatus>()
                    };
                    await _statusRepository.AddAsync(status);
                }
                return new Response<Status>(status);
            }
        }
    }
}
