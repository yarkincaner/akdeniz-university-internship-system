using AutoMapper;
using Internships.Core.Features.Internships.Queries.GetAllInternships;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Internships.Queries.GetInternshipsByStatus
{
    public class GetInternshipsByStatusQuery : IRequest<Response<IEnumerable<GetAllInternshipsViewModel>>>
    {
        public string StatusName { get; set; }
    }
    public class GetPendingInternshipsQueryHandler : IRequestHandler<GetInternshipsByStatusQuery, Response<IEnumerable<GetAllInternshipsViewModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IInternshipRepositoryAsync _internshipRepository;

        public GetPendingInternshipsQueryHandler(IMapper mapper, IInternshipRepositoryAsync internshipRepository)
        {
            _mapper = mapper;
            _internshipRepository = internshipRepository;
        }

        public async Task<Response<IEnumerable<GetAllInternshipsViewModel>>> Handle(GetInternshipsByStatusQuery request, CancellationToken cancellationToken)
        {
            var internships = await _internshipRepository.GetAllInternshipsByStatusAsync(request.StatusName);
            var internshipViewModel = _mapper.Map<IEnumerable<GetAllInternshipsViewModel>>(internships);
            return new Response<IEnumerable<GetAllInternshipsViewModel>>(internshipViewModel);
        }
    }
}
