using AutoMapper;
using Internships.Core.Exceptions;
using Internships.Core.Interfaces;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.InternshipStatuses.Queries.GetInternshipStatusesByInternshipId
{
    public class GetInternshipStatusesByInternshipIdQuery : IRequest<Response<IEnumerable<GetInternshipStatusesByInternshipIdViewModel>>>
    {
        public int InternshipId { get; set; }
    }

    public class GetInternshipStatusesByInternshipIdQueryHandler : IRequestHandler<GetInternshipStatusesByInternshipIdQuery, Response<IEnumerable<GetInternshipStatusesByInternshipIdViewModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IInternshipRepositoryAsync _internshipRepository;
        private readonly IInternshipStatusRepositoryAsync _internshipStatusRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public GetInternshipStatusesByInternshipIdQueryHandler(IMapper mapper, IInternshipRepositoryAsync internshipRepository, IInternshipStatusRepositoryAsync internshipStatusRepository, IAuthenticatedUserService authenticatedUserService)
        {
            _mapper = mapper;
            _internshipRepository = internshipRepository;
            _internshipStatusRepository = internshipStatusRepository;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<Response<IEnumerable<GetInternshipStatusesByInternshipIdViewModel>>> Handle(GetInternshipStatusesByInternshipIdQuery request, CancellationToken cancellationToken)
        {
            var internship = await _internshipRepository.GetByIdAsync(request.InternshipId);
            if (internship == null) throw new EntityNotFoundException("Internship", request.InternshipId);

            var internshipStatuses = await _internshipStatusRepository
                .GetStatusesByInternshipId(request.InternshipId);

            return new Response<IEnumerable<GetInternshipStatusesByInternshipIdViewModel>>(internshipStatuses);
        }
    }
}
