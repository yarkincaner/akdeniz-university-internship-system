using Internships.Core.Entities;
using Internships.Core.Enums;
using Internships.Core.Exceptions;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Internships.Commands.UpdateInternship
{
    public class UpdateInternshipCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }

        public class UpdateInternshipCommandHandler : IRequestHandler<UpdateInternshipCommand, Response<int>>
        {
            private readonly IInternshipRepositoryAsync _internshipRepository;
            private readonly IEmployeeRepositoryAsync _employeeRepository;
            private readonly IInternshipStatusRepositoryAsync _internshipStatusRepository;

            public UpdateInternshipCommandHandler(IInternshipRepositoryAsync internshipRepository, IEmployeeRepositoryAsync employeeRepository, IInternshipStatusRepositoryAsync internshipStatusRepository)
            {
                _internshipRepository = internshipRepository;
                _employeeRepository = employeeRepository;
                _internshipStatusRepository = internshipStatusRepository;
            }

            public async Task<Response<int>> Handle(UpdateInternshipCommand command, CancellationToken cancellationToken)
            {
                var internship = await _internshipRepository.GetByIdAsync(command.Id);
                if (internship == null) throw new EntityNotFoundException("Internship", command.Id);

                var employee = await _employeeRepository.GetByIdAsync(command.EmployeeId);
                if (employee == null) throw new EntityNotFoundException("Employee", command.EmployeeId);

                if (DateTime.Compare(command.EndDate, command.StartDate) <= 0) throw new Exception("Start date must be earlier than end date");
                if (command.TotalDays > 60 || command.TotalDays < 1) throw new Exception("Total days should be between 1-60");

                internship.EmployeeId = command.EmployeeId;
                internship.Employee = employee;
                internship.StartDate = command.StartDate;
                internship.EndDate = command.EndDate;
                internship.TotalDays = command.TotalDays;

                // Update status as pending approval from internship committee
                var newStatus = new InternshipStatus
                {
                    InternshipId = internship.Id,
                    StatusId = (int)InternshipStatusEnum.PendingApprovalFromInternshipCommittee,
                    IsEnabled = true
                };
                internship.InternshipStatuses.Add(newStatus);
                await _internshipStatusRepository.AddAsync(newStatus);

                await _internshipRepository.UpdateAsync(internship);
                return new Response<int>(internship.Id);
            }
        }
    }
}
