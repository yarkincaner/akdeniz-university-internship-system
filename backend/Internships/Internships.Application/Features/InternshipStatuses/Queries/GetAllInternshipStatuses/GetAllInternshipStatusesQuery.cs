using AutoMapper;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.InternshipStatuses.Queries.GetAllInternshipStatuses
{
    public class GetAllInternshipStatusesQuery : IRequest<IEnumerable<GetAllInternshipStatusesViewModel>>
    {
        public int InternshipId { get; set; }
    }

    public class GetAllInternshipStatusesQueryHandler : IRequestHandler<GetAllInternshipStatusesQuery, IEnumerable<GetAllInternshipStatusesViewModel>>
    {
        private readonly IInternshipStatusRepositoryAsync _internshipStatusRepository;

        public GetAllInternshipStatusesQueryHandler( IInternshipStatusRepositoryAsync internshipStatusRepository)
        {
            _internshipStatusRepository = internshipStatusRepository;
        }

        public async Task<IEnumerable<GetAllInternshipStatusesViewModel>> Handle(GetAllInternshipStatusesQuery request, CancellationToken cancellationToken)
        {
            return  await  _internshipStatusRepository.GetStatuses(request.InternshipId);
            
        }
    }
}
