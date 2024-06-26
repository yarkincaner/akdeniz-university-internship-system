using Internships.Core.Features.Companies.Commands.ApproveCompanyById;
using Internships.Core.Features.Companies.Commands.CreateCompany;
using Internships.Core.Features.Companies.Commands.DeleteCompanyById;
using Internships.Core.Features.Companies.Commands.UpdateCompany;
using Internships.Core.Features.Companies.Queries.GetAllCompanies;
using Internships.Core.Features.Companies.Queries.GetCompanyById;
using Internships.Core.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internships.WebApi.Controllers.v1
{
    [Authorize]
    public class CompanyController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<IEnumerable<GetAllCompaniesViewModel>>))]
        public async Task<PagedResponse<IEnumerable<GetAllCompaniesViewModel>>> Get([FromQuery] GetAllCompaniesParameter filter)
        {
            return await Mediator.Send(new GetAllCompaniesQuery()
            {
                SearchString = filter.SearchString,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetCompanyByIdQuery { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateCompanyCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateCompanyCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCompanyByIdCommand { Id = id }));
        }

        [HttpPut("Approve")]
        public async Task<IActionResult> Put(ApproveCompanyByIdCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
