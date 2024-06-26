using Internships.Core.Features.Departments.Commands.CreateDepartment;
using Internships.Core.Features.Departments.Commands.DeleteDepartmentById;
using Internships.Core.Features.Departments.Commands.UpdateDepartment;
using Internships.Core.Features.Departments.Queries.GetAllDepartments;
using Internships.Core.Features.Departments.Queries.GetDepartmentById;
using Internships.Core.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internships.WebApi.Controllers.v1
{
    [Authorize]
    public class DepartmentController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<IEnumerable<GetAllDepartmentsViewModel>>))]
        public async Task<PagedResponse<IEnumerable<GetAllDepartmentsViewModel>>> Get([FromQuery] GetAllDepartmentsParameter filter)
        {
            return await Mediator.Send(new GetAllDepartmentsQuery()
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber,
            });
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetDepartmentByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(CreateDepartmentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateDepartmentCommand command)
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
            return Ok(await Mediator.Send(new DeleteDepartmentByIdCommand { Id = id }));
        }
    }
}
