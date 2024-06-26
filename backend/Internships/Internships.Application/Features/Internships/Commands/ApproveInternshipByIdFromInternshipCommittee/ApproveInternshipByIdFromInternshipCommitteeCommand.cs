using Internships.Core.Entities;
using Internships.Core.Enums;
using Internships.Core.Exceptions;
using Internships.Core.Helpers;
using Internships.Core.Interfaces;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Internships.Commands.ApproveInternshipById
{
    public class ApproveInternshipByIdFromInternshipCommitteeCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public bool IsApproved { get; set; }
        public string Comment { get; set; }

    }
    public class ApproveInternshipByIdFromInternshipCommitteeCommandHandler : IRequestHandler<ApproveInternshipByIdFromInternshipCommitteeCommand, Response<int>>
    {
        private readonly IInternshipRepositoryAsync _internshipRepository;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private readonly IEmailService _emailService;
        private readonly IExternalAccountService _externalAccountService;
        private readonly IEmployeeRepositoryAsync _employeeRepositoryAsync;
        private readonly IInternshipStatusRepositoryAsync _internshipStatusRepositoryAsync;

        public ApproveInternshipByIdFromInternshipCommitteeCommandHandler(
            IInternshipRepositoryAsync internshipRepository,
            IAuthenticatedUserService authenticatedUser,
            IEmailService emailService,
            IExternalAccountService externalAccountService,
            IEmployeeRepositoryAsync employeeRepositoryAsync,
            IInternshipStatusRepositoryAsync internshipStatusRepositoryAsync)
        {
            _internshipRepository = internshipRepository;
            _authenticatedUser = authenticatedUser;
            _emailService = emailService;
            _externalAccountService = externalAccountService;
            _employeeRepositoryAsync = employeeRepositoryAsync;
            _internshipStatusRepositoryAsync = internshipStatusRepositoryAsync;
        }
        public async Task<Response<int>> Handle(ApproveInternshipByIdFromInternshipCommitteeCommand command, CancellationToken cancellationToken)
        {
            var internship = await _internshipRepository.GetByIdAsync(command.Id);
            if (internship == null)
            {
                throw new EntityNotFoundException("Internship", command.Id);
            }

            var isAlreadyApproved = internship.InternshipStatuses
                .FirstOrDefault(t => t.IsEnabled && t.StatusId != (int)InternshipStatusEnum.PendingApprovalFromInternshipCommittee);

            if (isAlreadyApproved != null)
            {
                throw new Exception("Internship already approved or declined by the committee");
            }

            internship.ApprovedBy = _authenticatedUser.UserId;
            foreach (var item in internship.InternshipStatuses.Where(stat => stat.IsEnabled))
            {
                item.IsEnabled = false;
                _internshipStatusRepositoryAsync.AttachModified(item);
            }

            if (!command.IsApproved)
            {
                if (command.Comment.Length < 10)
                {
                    throw new ApiException("Comment can not be less than 10 characters");
                }

                var declinedStatus = new InternshipStatus
                {
                    InternshipId = internship.Id,
                    StatusId = (int)InternshipStatusEnum.DeclinedFromInternshipCommitte,
                    IsEnabled = true,
                    Comment = command.Comment,
                };

                await _internshipStatusRepositoryAsync.AddAsync(declinedStatus);
                return new Response<int>(internship.Id);
                //TODO: send mail to student.
            }


            var approvedStatus = new InternshipStatus
            {
                InternshipId = internship.Id,
                StatusId = (int)InternshipStatusEnum.AcceptedByInternshipCommittee,
                IsEnabled = true,
            };

            internship.InternshipStatuses.Add(approvedStatus);
            _internshipStatusRepositoryAsync.AttachAdded(approvedStatus);

            var employee = await _employeeRepositoryAsync.GetByIdAsync(internship.EmployeeId);
            if (employee == null)
            {
                throw new EntityNotFoundException("Employee", internship.EmployeeId);
            }
            await _internshipRepository.UpdateAsync(internship);


            string token = _externalAccountService.GenerateToken(employee.Email, internship.Id);
            string emailTemplate = MailTemplateHelper.CreateCompanyApprovalMail(
                $"{employee.FirstName} {employee.LastName}",
                $"{internship.User.FirstName} {internship.User.LastName}",
                token);
            await _emailService.SendEmail(employee.Email, emailTemplate);
            foreach (var item in internship.InternshipStatuses.Where(stat => stat.IsEnabled))
            {
                item.IsEnabled = false;
                _internshipStatusRepositoryAsync.AttachModified(item);
            }

            var pendingApprovalCompanyStatus = new InternshipStatus
            {
                InternshipId = internship.Id,
                StatusId = (int)InternshipStatusEnum.PendingApprovalFromCompany,
            };

            await _internshipStatusRepositoryAsync.AddAsync(pendingApprovalCompanyStatus);
            return new Response<int>(command.Id);
        }
    }


}
