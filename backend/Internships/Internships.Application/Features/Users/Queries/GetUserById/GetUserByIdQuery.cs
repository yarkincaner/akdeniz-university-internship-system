using AutoMapper;
using Internships.Core.Entities;
using Internships.Core.Exceptions;
using Internships.Core.Interfaces;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<Response<User>>
    {

    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Response<User>>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepositoryAsync _userRepository;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public GetUserByIdQueryHandler(IMapper mapper, IUserRepositoryAsync userRepository, IAuthenticatedUserService authenticatedUser)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _authenticatedUser = authenticatedUser;
        }

        public async Task<Response<User>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var userId = _authenticatedUser.UserId;
            if (userId == null)
            {
                throw new ApiException("User is not authenticated!");
            }

            var user = await _userRepository.GetByUserId(userId);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with id {userId} not found!");
            }

            return new Response<User>(user);
        }
    }
}
