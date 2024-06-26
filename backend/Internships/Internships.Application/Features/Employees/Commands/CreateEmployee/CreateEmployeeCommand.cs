using AutoMapper;
using Internships.Core.Entities;
using Internships.Core.Exceptions;
using Internships.Core.Interfaces;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Employees.Commands.CreateEmployee
{
    public class CreateEmployeeCommand : IRequest<Response<int>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }

        public int CompanyId { get; set; }
    }

    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Response<int>>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeRepositoryAsync _employeeRepository;
        private readonly ICompanyRepositoryAsync _companyRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IUserRepositoryAsync _userRepository;
        public CreateEmployeeCommandHandler(IMapper mapper, IEmployeeRepositoryAsync employeeRepository, ICompanyRepositoryAsync companyRepository, IAuthenticatedUserService authenticatedUserService, IUserRepositoryAsync userRepository)
        {
            _mapper = mapper;
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _authenticatedUserService = authenticatedUserService;
            _userRepository = userRepository;
        }

        public async Task<Response<int>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var userId = _authenticatedUserService.UserId;
            if (userId == null) throw new ApiException("Could not authenticated user!");

            var user = await _userRepository.GetByUserId(userId);
            if (user == null) throw new EntityNotFoundException("User", userId);
            if (user.BirthYear == 0 || user.TcKimlikNo == 0) throw new ApiException("Tc kimlik no or Birthyear is missing!");

            var company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company == null) throw new EntityNotFoundException("Company", request.CompanyId);
            var employee = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Title = request.Title,
                Email = request.Email,
                CompanyId = request.CompanyId,
                IsEnabled = true
            };
            await _employeeRepository.AddAsync(employee);
            return new Response<int>(employee.Id);
        }
    }
}
