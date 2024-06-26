using AutoMapper;
using Internships.Core.Entities;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Users.Commands.CreateUser
{
    public class CreateUserCommand :IRequest<Response<string>>
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentId { get; set; }
        
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response<string>>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepositoryAsync _userRepository;
        
        public CreateUserCommandHandler(IMapper mapper,IUserRepositoryAsync userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<Response<string>> Handle(CreateUserCommand request,CancellationToken cancellationToken)
        {
            var user = new User
            {
                UserId = request.UserId,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DepartmentID = request.DepartmentId,
                Created = DateTime.Now,
                IsEnabled = true
            };
            await _userRepository.AddAsync(user);
            return new Response<string>(user.UserId);
        }
    }
}
