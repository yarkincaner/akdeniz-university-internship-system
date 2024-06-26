using Internships.Core.Entities;
using Internships.Core.Exceptions;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Internships.Core.Features.Employees.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }

        public int CompanyId { get; set; }

        public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Response<int>>
        {
            private readonly IEmployeeRepositoryAsync _employeeRepository;
            private readonly ICompanyRepositoryAsync _companyRepository;

            public UpdateEmployeeCommandHandler(IEmployeeRepositoryAsync employeeRepository, ICompanyRepositoryAsync companyRepository)
            {
                _employeeRepository = employeeRepository;
                _companyRepository = companyRepository;
            }

            public async Task<Response<int>> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
            {
                var employee = await _employeeRepository.GetByIdAsync(command.Id);

                if (employee == null)
                {
                    throw new EntityNotFoundException("Employee", command.Id);
                }

                var company = await _companyRepository.GetByIdAsync(command.CompanyId);
                if (company == null)
                {
                    throw new EntityNotFoundException("Company", command.CompanyId);
                }

                employee.FirstName = command.FirstName;
                employee.LastName = command.LastName;
                employee.Email = command.Email;
                employee.CompanyId = command.CompanyId;

                await _employeeRepository.UpdateAsync(employee);
                return new Response<int>(employee.Id);
            }
        }
    }
}
