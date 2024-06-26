using AutoMapper;
using Internships.Core.Entities;
using Internships.Core.Exceptions;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.InternshipStatuses.Commands.CreateInternshipStatus
{
    public class CreateInternshipStatusCommand : IRequest<Response<int>>
    {
        public int InternshipId { get; set; }
        public int StatusId { get; set; }

        public string Comment { get; set; }
    }

    public class CreateInternshipStatusCommandHandler : IRequestHandler<CreateInternshipStatusCommand, Response<int>>
    {
        private readonly IMapper _mapper;
        private readonly IInternshipStatusRepositoryAsync _internshipStatusRepository;
        private readonly IInternshipRepositoryAsync _internshipRepository;
        private readonly IStatusRepositoryAsync _statusRepository;
        public CreateInternshipStatusCommandHandler(IMapper mapper, IInternshipStatusRepositoryAsync internshipStatusRepository, IInternshipRepositoryAsync internshipRepository, IStatusRepositoryAsync statusRepository)
        {
            _mapper = mapper;
            _internshipStatusRepository = internshipStatusRepository;
            _internshipRepository = internshipRepository;
            _statusRepository = statusRepository;
        }

        public async Task<Response<int>> Handle(CreateInternshipStatusCommand request, CancellationToken cancellationToken)
        {
            var internship = await _internshipRepository.GetByIdAsync(request.InternshipId);
            if (internship == null) throw new EntityNotFoundException("Internship", request.InternshipId);
            
            var status = await _statusRepository.GetByIdAsync(request.StatusId);
            if (status == null) throw new EntityNotFoundException("Status", request.StatusId);
            
            var internshipStatus = new InternshipStatus
            {
                InternshipId = internship.Id,
                StatusId = status.Id,
                Comment = request.Comment,
            };
            await _internshipStatusRepository.AddAsync(internshipStatus);
            return new Response<int>(internship.Id);
        }
    }
}
