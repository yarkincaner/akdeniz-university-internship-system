using Internships.Core.Exceptions;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Internships.Commands.DeleteInternshipById
{
    public class DeleteInternshipByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }

        public class DeleteInternshipByIdCommandHandler : IRequestHandler<DeleteInternshipByIdCommand, Response<int>>
        {
            private readonly IInternshipRepositoryAsync _internshipRepository;

            public DeleteInternshipByIdCommandHandler(IInternshipRepositoryAsync internshipRepository)
            {
                _internshipRepository = internshipRepository;
            }

            public async Task<Response<int>> Handle(DeleteInternshipByIdCommand command, CancellationToken cancellationToken)
            {
                var internship = await _internshipRepository.GetByIdAsync(command.Id);
                if (internship == null) throw new EntityNotFoundException("Internship", command.Id);
                internship.IsEnabled = false;
                await _internshipRepository.UpdateAsync(internship);
                return new Response<int>(internship.Id);
            }
        }
    }
}
