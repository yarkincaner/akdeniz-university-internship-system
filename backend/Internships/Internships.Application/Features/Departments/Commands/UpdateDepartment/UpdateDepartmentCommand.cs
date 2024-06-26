using Internships.Core.Exceptions;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Internships.Core.Features.Departments.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, Response<int>>
        {
            private readonly IDepartmentRepositoryAsync _departmentRepository;

            public UpdateDepartmentCommandHandler(IDepartmentRepositoryAsync departmentRepository)
            {
                _departmentRepository = departmentRepository;
            }

            public async Task<Response<int>> Handle(UpdateDepartmentCommand command, CancellationToken cancellationToken)
            {
                var department = await _departmentRepository.GetByIdAsync(command.Id);

                if (department == null)
                {
                    throw new EntityNotFoundException("Department", command.Id);
                }

                department.Name = command.Name;

                await _departmentRepository.UpdateAsync(department);
                return new Response<int>(department.Id);
            }
        }
    }
}
