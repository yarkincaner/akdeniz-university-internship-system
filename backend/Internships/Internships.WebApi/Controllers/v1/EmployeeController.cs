using Internships.Core.Features.Employees.Commands.CreateEmployee;
using Internships.Core.Features.Employees.Commands.DeleteEmployeeById;
using Internships.Core.Features.Employees.Commands.UpdateEmployee;
using Internships.Core.Features.Employees.Queries.GetAllEmployees;
using Internships.Core.Features.Employees.Queries.GetEmployeeById;
using Internships.Core.Features.Employees.Queries.GetEmployeesByCompanyId;
using Internships.Core.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internships.WebApi.Controllers.v1
{
    [Authorize]
    public class EmployeeController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<IEnumerable<GetAllEmployeesViewModel>>))]
        public async Task<PagedResponse<IEnumerable<GetAllEmployeesViewModel>>> Get([FromQuery] GetAllEmployeesParameter filter)
        {
            return await Mediator.Send(new GetAllEmployeesQuery()
            {
                SearchString = filter.SearchString,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
            });
        }

        // GET: api/<controller>/Company/5
        [HttpGet("Company/{companyId}")]
        public async Task<Response<IEnumerable<GetAllEmployeesViewModel>>> GetByCompanyId(int companyId, [FromQuery] GetAllEmployeesParameter filter)
        {
            return await Mediator.Send(new GetEmployeesByCompanyIdQuery()
            {
                CompanyId = companyId,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                SearchString = filter.SearchString,
            });
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetEmployeeByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateEmployeeCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteEmployeeByIdCommand { Id = id }));
        }
    }
}
