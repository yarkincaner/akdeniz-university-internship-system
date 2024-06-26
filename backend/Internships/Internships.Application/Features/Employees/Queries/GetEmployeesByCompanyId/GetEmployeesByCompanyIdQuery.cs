using AutoMapper;
using Internships.Core.Exceptions;
using Internships.Core.Features.Employees.Queries.GetAllEmployees;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Employees.Queries.GetEmployeesByCompanyId
{
    public class GetEmployeesByCompanyIdQuery : IRequest<PagedResponse<IEnumerable<GetAllEmployeesViewModel>>>
    {
        public int CompanyId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
    }

    public class GetEmployeesByCompanyIdQueryHandler : IRequestHandler<GetEmployeesByCompanyIdQuery, PagedResponse<IEnumerable<GetAllEmployeesViewModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepositoryAsync _employeeRepository;
        private readonly ICompanyRepositoryAsync _companyRepository;

        public GetEmployeesByCompanyIdQueryHandler(IMapper mapper, IEmployeeRepositoryAsync employeeRepository, ICompanyRepositoryAsync companyRepository)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
        }

        public async Task<PagedResponse<IEnumerable<GetAllEmployeesViewModel>>> Handle(GetEmployeesByCompanyIdQuery request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company == null)
            {
                throw new EntityNotFoundException("Company", request.CompanyId);
            }

            var validFilter = _mapper.Map<GetAllEmployeesParameter>(request);
            var employees = await _employeeRepository.GetPagedResponseByCompanyIdAsync(request.CompanyId, validFilter.PageNumber, validFilter.PageSize, request.SearchString);
            var totalCount = employees.TotalCount;
            return new PagedResponse<IEnumerable<GetAllEmployeesViewModel>>(employees.Data, validFilter.PageNumber, validFilter.PageSize, totalCount);
        }
    }
}
