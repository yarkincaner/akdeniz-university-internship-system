using Internships.Core.Features.Internships.Commands.ApproveInternshipByIdFromCompany;
using Internships.Core.Features.Internships.Queries.GetInternshipById;
using Internships.Core.Features.Internships.Queries.GetInternshipByIdExternalService;
using Internships.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Internships.WebApi.Controllers.v1
{
    public class ExternalAccountContoller:BaseApiController
    {
        private readonly IExternalAccountService _externalAccountService;

        public ExternalAccountContoller(IExternalAccountService externalAccountService)
        {
            _externalAccountService = externalAccountService;
        }
        [HttpGet("Decode-Token")]
        public async Task<IActionResult> Get(string token)
        {
            return Ok(await Mediator.Send(new GetInternshipByIdExternalServiceQuery { token=token }));
        }
        [HttpPost("Approve-Company")]
        public async Task<IActionResult> ApproveInternshipByCompany(ApproveInternshipByIdFromCompanyCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
