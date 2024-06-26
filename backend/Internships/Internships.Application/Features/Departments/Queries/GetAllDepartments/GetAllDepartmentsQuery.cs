using AutoMapper;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Departments.Queries.GetAllDepartments
{
    public class GetAllDepartmentsQuery : IRequest<PagedResponse<IEnumerable<GetAllDepartmentsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, PagedResponse<IEnumerable<GetAllDepartmentsViewModel>>>
    {
        private readonly IMapper _mapper;
        private readonly IDepartmentRepositoryAsync _departmentRepository;

        public GetAllDepartmentsQueryHandler(IMapper mapper, IDepartmentRepositoryAsync departmentRepository)
        {
            _mapper = mapper;
            _departmentRepository = departmentRepository;
        }

        public async Task<PagedResponse<IEnumerable<GetAllDepartmentsViewModel>>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllDepartmentsParameter>(request);
            var department = await _departmentRepository.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var departmentViewModel = _mapper.Map<IEnumerable<GetAllDepartmentsViewModel>>(department);
            return new PagedResponse<IEnumerable<GetAllDepartmentsViewModel>>(departmentViewModel, validFilter.PageNumber, validFilter.PageSize, department.Count);
        }
    }
}
