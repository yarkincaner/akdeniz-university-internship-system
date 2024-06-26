using AutoMapper;
using Internships.Core.Entities;
using Internships.Core.Enums;
using Internships.Core.Exceptions;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Internships.Commands.CreateInternship
{
    public class CreateInternshipCommand : IRequest<Response<int>>
    {
        public int CompanyId { get; set; }
        public int EmployeeId { get; set; }
        public string UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public InsuranceType InsuranceType { get; set; }
    }

    public class CreateInternshipCommandHandler : IRequestHandler<CreateInternshipCommand, Response<int>>
    {
        private readonly IMapper _mapper;
        private readonly IInternshipRepositoryAsync _internshipRepository;
        private readonly ICompanyRepositoryAsync _companyRepository;
        private readonly IEmployeeRepositoryAsync _employeeRepository;
        private readonly IUserRepositoryAsync _userRepository;
        public CreateInternshipCommandHandler(IMapper mapper, IInternshipRepositoryAsync internshipRepository, ICompanyRepositoryAsync companyRepository, IEmployeeRepositoryAsync employeeRepository, IUserRepositoryAsync userRepository)
        {
            _mapper = mapper;
            _internshipRepository = internshipRepository;
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
        }

        public async Task<Response<int>> Handle(CreateInternshipCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserId(request.UserId);
            if (user == null) throw new EntityNotFoundException("User", request.UserId);
            if (user.TcKimlikNo == 0 || user.BirthYear == 0) throw new ApiException("Tc kimlik no or Birthyear information is missing");

            var company = await _companyRepository.GetByIdAsync(request.CompanyId);
            if (company == null) throw new EntityNotFoundException("Company", request.CompanyId);

            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee == null) throw new EntityNotFoundException("Employee", request.EmployeeId);

            if (employee.CompanyId != request.CompanyId) throw new Exception($"Employee: ({employee.Id}) is not an employee of Company: ({company.Id}).");

            if (DateTime.Compare(request.EndDate, request.StartDate) <= 0) throw new ApiException("Start date must be earlier than end date");
            if (request.TotalDays > 60 || request.TotalDays <= 0) throw new ApiException("Total days should be between 1-60");

            if (!Enum.IsDefined(typeof(InsuranceType), request.InsuranceType)) throw new EntityNotFoundException("InsuranceType", request.InsuranceType);

            var internship = new Internship
            {
                CompanyId = request.CompanyId,
                EmployeeId = request.EmployeeId,
                UserId = request.UserId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TotalDays = request.TotalDays,
                InsuranceType = request.InsuranceType,
                IsApproved = false,
            };

            internship.InternshipStatuses.Add(new InternshipStatus
            {
                StatusId = (int)InternshipStatusEnum.PendingApprovalFromInternshipCommittee,
                IsEnabled = true,
            });

            await _internshipRepository.AddAsync(internship);

            return new Response<int>(internship.Id);
        }
    }
}
