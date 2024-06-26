using AutoMapper;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Employees.Queries.GetAllEmployees
{
    public class GetAllEmployeesQuery : IRequest<PagedResponse<IEnumerable<GetAllEmployeesViewModel>>>
    {
        public string SearchString { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, PagedResponse<IEnumerable<GetAllEmployeesViewModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepositoryAsync _employeeRepository;

        public GetAllEmployeesQueryHandler(IMapper mapper, IEmployeeRepositoryAsync employeeRepository)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
        }

        public async Task<PagedResponse<IEnumerable<GetAllEmployeesViewModel>>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllEmployeesParameter>(request);
            var employees = await _employeeRepository.GetPagedResponseAsync(validFilter.PageNumber, validFilter.PageSize, request.SearchString);
            var totalCount = employees.TotalCount;
            return new PagedResponse<IEnumerable<GetAllEmployeesViewModel>>(employees.Data, validFilter.PageNumber, validFilter.PageSize, totalCount);
        }
    }
}
