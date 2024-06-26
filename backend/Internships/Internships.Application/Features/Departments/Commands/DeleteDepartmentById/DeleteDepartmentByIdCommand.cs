using Internships.Core.Exceptions;
using Internships.Core.Features.Companies.Commands.DeleteCompanyById;
using Internships.Core.Interfaces.Repositories;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Internships.Core.Wrappers;

namespace Internships.Core.Features.Departments.Commands.DeleteDepartmentById
{
    public class DeleteDepartmentByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }

        public class DeleteDepartmentByIdCommandHandler : IRequestHandler<DeleteDepartmentByIdCommand, Response<int>>
        {
            private readonly IDepartmentRepositoryAsync _departmentRepository;

            public DeleteDepartmentByIdCommandHandler(IDepartmentRepositoryAsync departmentRepository)
            {
                _departmentRepository = departmentRepository;
            }

            public async Task<Response<int>> Handle(DeleteDepartmentByIdCommand command, CancellationToken cancellationToken)
            {
                var department = await _departmentRepository.GetByIdAsync(command.Id);
                if (department == null) throw new ApiException("Department Not Found.");
                department.IsEnabled = false;
                await _departmentRepository.UpdateAsync(department);
                return new Response<int>(department.Id);
            }
        }
    }
}
