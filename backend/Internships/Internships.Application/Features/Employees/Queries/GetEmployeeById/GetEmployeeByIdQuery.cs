using Internships.Core.Exceptions;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Employees.Queries.GetEmployeeById
{
    public class GetEmployeeByIdQuery : IRequest<Response<GetEmployeeByIdViewModel>>
    {
        public int Id { get; set; }

        public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Response<GetEmployeeByIdViewModel>>
        {
            private readonly IEmployeeRepositoryAsync _employeeRepository;

            public GetEmployeeByIdQueryHandler(IEmployeeRepositoryAsync employeeRepository)
            {
                _employeeRepository = employeeRepository;
            }

            public async Task<Response<GetEmployeeByIdViewModel>> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
            {
                var employee = await _employeeRepository.GetEmployeeById(query.Id);
                if (employee == null)
                {
                    throw new EntityNotFoundException("Employee Not Found.");
                }
                return new Response<GetEmployeeByIdViewModel>(employee);
            }
        }
    }
}
