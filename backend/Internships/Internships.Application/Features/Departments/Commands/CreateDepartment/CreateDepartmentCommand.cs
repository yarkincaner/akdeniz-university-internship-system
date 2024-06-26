using AutoMapper;
using Internships.Core.Entities;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Threading.Tasks;
using System.Threading;

namespace Internships.Core.Features.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
    }

    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Response<int>>
    {
        private readonly IMapper _mapper;
        private readonly IDepartmentRepositoryAsync _departmentRepository;
        public CreateDepartmentCommandHandler(IMapper mapper, IDepartmentRepositoryAsync departmentRepository)
        {
            _mapper = mapper;
            _departmentRepository = departmentRepository;
        }

        public async Task<Response<int>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = new Department
            {
                Name = request.Name,
                IsEnabled = true
            };
            await _departmentRepository.AddAsync(department);
            return new Response<int>(department.Id);
        }
    }
}
