using AutoMapper;
using Internships.Core.Features.Internships.Queries.GetAllInternships;
using Internships.Core.Interfaces;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Internships.Queries.GetInternshipsByUserId
{
    public class GetInternshipsByUserIdQuery : IRequest<PagedResponse<IEnumerable<GetInternshipsByUserIdViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

    }
    public class GetInternshipsByUserIdQueryHandler : IRequestHandler<GetInternshipsByUserIdQuery, PagedResponse<IEnumerable<GetInternshipsByUserIdViewModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IInternshipRepositoryAsync _internshipRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public GetInternshipsByUserIdQueryHandler(IMapper mapper,
            IInternshipRepositoryAsync internshipRepository,
            IAuthenticatedUserService authenticatedUserService)
        {
            _mapper = mapper;
            _internshipRepository = internshipRepository;
            _authenticatedUserService = authenticatedUserService;
        }

        public async Task<PagedResponse<IEnumerable<GetInternshipsByUserIdViewModel>>> Handle(GetInternshipsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllInternshipsParameter>(request);
            var internships = await _internshipRepository.GetInternshipByUserIdAsync(validFilter.PageNumber, validFilter.PageSize, _authenticatedUserService.UserId);
            var totalCount = internships.Count;
            var internshipViewModel = _mapper.Map<IEnumerable<GetInternshipsByUserIdViewModel>>(internships);

            return new PagedResponse<IEnumerable<GetInternshipsByUserIdViewModel>>(internshipViewModel, validFilter.PageNumber, validFilter.PageSize, totalCount);
        }
    }




}

