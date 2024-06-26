using Internships.Core.Exceptions;
using Internships.Core.Interfaces.Repositories;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Internships.Core.Wrappers;

namespace Internships.Core.Features.Employees.Commands.DeleteEmployeeById
{
    public class DeleteEmployeeByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }

        public class DeleteEmployeeByIdCommandHandler : IRequestHandler<DeleteEmployeeByIdCommand, Response<int>>
        {
            private readonly IEmployeeRepositoryAsync _employeeRepository;

            public DeleteEmployeeByIdCommandHandler(IEmployeeRepositoryAsync employeeRepository)
            {
                _employeeRepository = employeeRepository;
            }

            public async Task<Response<int>> Handle(DeleteEmployeeByIdCommand command, CancellationToken cancellationToken)
            {
                var employee = await _employeeRepository.GetByIdAsync(command.Id);
                if (employee == null) throw new EntityNotFoundException($"The employee with id {command.Id} was not found)");
                employee.IsEnabled = false;
                await _employeeRepository.UpdateAsync(employee);
                return new Response<int>(employee.Id);
            }
        }
    }
}
