using Internships.Core.Entities;
using Internships.Core.Exceptions;
using Internships.Core.Interfaces.Repositories;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Internships.Core.Wrappers;

namespace Internships.Core.Features.Departments.Queries.GetDepartmentById
{
    public class GetDepartmentByIdQuery : IRequest<Response<Department>>
    {
        public int Id { get; set; }

        public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, Response<Department>>
        {
            private readonly IDepartmentRepositoryAsync _departmentRepository;

            public GetDepartmentByIdQueryHandler(IDepartmentRepositoryAsync departmentRepository)
            {
                _departmentRepository = departmentRepository;
            }

            public async Task<Response<Department>> Handle(GetDepartmentByIdQuery query, CancellationToken cancellationToken)
            {
                var department = await _departmentRepository.GetByIdAsync(query.Id);
                if (department == null)
                {
                    throw new ApiException("Department Not Found.");
                }
                return new Response<Department>(department);
            }
        }
    }
}
