using AutoMapper;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Companies.Queries.GetAllCompanies
{
    public class GetAllCompaniesQuery : IRequest<PagedResponse<IEnumerable<GetAllCompaniesViewModel>>>
    {
        public string SearchString { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, PagedResponse<IEnumerable<GetAllCompaniesViewModel>>>
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepositoryAsync _companyRepository;

        public GetAllCompaniesQueryHandler(IMapper mapper, ICompanyRepositoryAsync companyRepository)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
        }

        public async Task<PagedResponse<IEnumerable<GetAllCompaniesViewModel>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllCompaniesParameter>(request);
            var companies = await _companyRepository.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize, request.SearchString);
            var totalCount = companies.TotalCount;
            return new PagedResponse<IEnumerable<GetAllCompaniesViewModel>>(companies.Data, validFilter.PageNumber, validFilter.PageSize, totalCount);
        }
    }
}
