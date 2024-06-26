using Internships.Core.Interfaces;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internships.Core.Features.Internships.Queries.GetInternshipByIdExternalService
{
    public class GetInternshipByIdExternalServiceQuery:IRequest<Response<GetInternshipByIdExternalServiceViewModel>>
    {
        public string token { get; set; }
    }
    public class GetInternshipByIdExternalServiceQueryHandler : IRequestHandler<GetInternshipByIdExternalServiceQuery, Response<GetInternshipByIdExternalServiceViewModel>>
    {
        private readonly IInternshipRepositoryAsync _internshipRepositoryAsync;
        private readonly IExternalAccountService _externalAccountService;

        public GetInternshipByIdExternalServiceQueryHandler(IInternshipRepositoryAsync internshipRepositoryAsync,IExternalAccountService externalAccountService)
        {
            _internshipRepositoryAsync = internshipRepositoryAsync;
            _externalAccountService = externalAccountService;
        }

        public async Task<Response<GetInternshipByIdExternalServiceViewModel>> Handle(GetInternshipByIdExternalServiceQuery request, CancellationToken cancellationToken)
        {
            int id=_externalAccountService.DecodeToken(request.token);
            return await _internshipRepositoryAsync.GetInternshipByIdExternalServiceAsync(id);
        }
    }
}
