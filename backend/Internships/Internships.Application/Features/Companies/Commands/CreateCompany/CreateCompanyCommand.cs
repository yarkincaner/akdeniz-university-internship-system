using AutoMapper;
using Internships.Core.Entities;
using Internships.Core.Exceptions;
using Internships.Core.Interfaces;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Companies.Commands.CreateCompany
{
    public class CreateCompanyCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        //TODO: Address should be a complex type, in a different table. We should refence it here.
        public string Address { get; set; }
        public string ServiceArea { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string TaxNumber { get; set; }

    }

    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Response<int>>
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepositoryAsync _companyRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IUserRepositoryAsync _userRepository;
        public CreateCompanyCommandHandler(IMapper mapper, ICompanyRepositoryAsync companyRepository, IAuthenticatedUserService authenticatedUserService, IUserRepositoryAsync userRepository)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
            _authenticatedUserService = authenticatedUserService;
            _userRepository = userRepository;
        }

        public async Task<Response<int>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var userId = _authenticatedUserService.UserId;
            if (userId == null) throw new ApiException("Could not authenticated user!");

            var user = await _userRepository.GetByUserId(userId);
            if (user == null) throw new EntityNotFoundException("User", userId);
            if (user.BirthYear == 0 || user.TcKimlikNo == 0) throw new ApiException("Tc kimlik no or Birthyear is missing!");

            var company = new Company
            {
                Name = request.Name,
                Address = request.Address,
                ServiceArea = request.ServiceArea,
                Phone = request.Phone,
                Email = request.Email,
                Website = request.Website,
                TaxNumber = request.TaxNumber,
                IsEnabled = true,
                IsApproved = false,
            };
            await _companyRepository.AddAsync(company);
            return new Response<int>(company.Id);
        }
    }
}
