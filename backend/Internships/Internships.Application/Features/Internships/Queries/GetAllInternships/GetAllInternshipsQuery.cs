using AutoMapper;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Internships.Queries.GetAllInternships
{
    public class GetAllInternshipsQuery : IRequest<PagedResponse<IEnumerable<GetAllInternshipsViewModel>>>
    {
        public string SearchString { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllInternshipsQueryHandler : IRequestHandler<GetAllInternshipsQuery, PagedResponse<IEnumerable<GetAllInternshipsViewModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IInternshipRepositoryAsync _internshipRepository;

        public GetAllInternshipsQueryHandler(IMapper mapper, IInternshipRepositoryAsync internshipRepository)
        {
            _mapper = mapper;
            _internshipRepository = internshipRepository;
        }

        public async Task<PagedResponse<IEnumerable<GetAllInternshipsViewModel>>> Handle(GetAllInternshipsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllInternshipsParameter>(request);
            var internships = await _internshipRepository.GetPagedResponseAsync(validFilter.PageNumber, validFilter.PageSize, validFilter.SearchString);
            var totalCount = internships.TotalCount;
            return new PagedResponse<IEnumerable<GetAllInternshipsViewModel>>(internships.Data, validFilter.PageNumber, validFilter.PageSize, totalCount);
        }
    }
}
