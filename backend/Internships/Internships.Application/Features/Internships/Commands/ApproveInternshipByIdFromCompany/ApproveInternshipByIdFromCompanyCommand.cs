
using Internships.Core.Exceptions;
using Internships.Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Internships.Core.Entities;
using Internships.Core.Enums;
using Internships.Core.Wrappers;
using System.Linq;

namespace Internships.Core.Features.Internships.Commands.ApproveInternshipByIdFromCompany
{
    public class ApproveInternshipByIdFromCompanyCommand:IRequest<Response<int>>
    {
        public int Id { get; set; }
        public bool IsApproved { get; set; }
    }
    public class ApproveInternshipByIdFromCompanyCommandHandler : IRequestHandler<ApproveInternshipByIdFromCompanyCommand, Response<int>>
    {
        private readonly IInternshipRepositoryAsync _internshipRepositoryAsync;
        private readonly IInternshipStatusRepositoryAsync _internshipStatusRepositoryAsync;

        public ApproveInternshipByIdFromCompanyCommandHandler(IInternshipRepositoryAsync internshipRepositoryAsync, IInternshipStatusRepositoryAsync internshipStatusRepositoryAsync)
        {
            _internshipRepositoryAsync = internshipRepositoryAsync;
            _internshipStatusRepositoryAsync = internshipStatusRepositoryAsync;
        }

        public async Task<Response<int>> Handle(ApproveInternshipByIdFromCompanyCommand command, CancellationToken cancellationToken)
        {
            var internship=await _internshipRepositoryAsync.GetInternshipsWithStatus(command.Id);
            if (internship == null)
            {
                throw new EntityNotFoundException("Internship", command.Id);
            }
            foreach (var item in internship.InternshipStatuses.Where(stat=>stat.IsEnabled))
            {
                item.IsEnabled = false;
                _internshipStatusRepositoryAsync.AttachModified(item);
            }
            if (command.IsApproved) {
                var acceptedStatusCompany = new InternshipStatus
                {
                    InternshipId=internship.Id,
                    StatusId = (int)InternshipStatusEnum.AcceptedByCompany,
                    IsEnabled=true
                };
                internship.InternshipStatuses.Add(acceptedStatusCompany);
                await _internshipStatusRepositoryAsync.AddAsync(acceptedStatusCompany);
            }
            else
            {
                var declinedStatusCompany = new InternshipStatus
                {
                    InternshipId = internship.Id,
                    StatusId = (int)InternshipStatusEnum.DeclinedFromCompany,
                    IsEnabled = true
                };
                internship.InternshipStatuses.Add(declinedStatusCompany);
                await _internshipStatusRepositoryAsync.AddAsync(declinedStatusCompany);
            }
            await _internshipRepositoryAsync.UpdateAsync(internship);
            return new Response<int>(internship.Id);
        }
    }
}
