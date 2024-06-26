using AutoMapper;
using Internships.Core.Entities;
using Internships.Core.Interfaces.Repositories;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Internships.Core.Wrappers;
using System.Collections.Generic;
using Internships.Core.Enums;

namespace Internships.Core.Features.Statuses.Commands.CreateStatus
{
    public class CreateStatusCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
    }

    public class CreateStatusCommandHandler : IRequestHandler<CreateStatusCommand, Response<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStatusRepositoryAsync _statusRepository;
        public CreateStatusCommandHandler(IMapper mapper, IStatusRepositoryAsync statusRepository)
        {
            _mapper = mapper;
            _statusRepository = statusRepository;
        }

        public async Task<Response<int>> Handle(CreateStatusCommand request, CancellationToken cancellationToken)
        {
            var status = new Status
            {
                Name = request.Name
            };
            await _statusRepository.AddAsync(status);
            return new Response<int>(status.Id);
        }
    }
}
