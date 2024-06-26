using Internships.Core.Features.Users.Commands.ConfirmCitizienship;
using Internships.Core.Features.Users.Commands.CreateUser;
using Internships.Core.Features.Users.Commands.UpdateUser;
using Internships.Core.Features.Users.Queries.GetUserById;
using Internships.Core.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Internships.WebApi.Controllers.v1;

[ApiVersion("1.0")]
public class UserController : BaseApiController
{
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [HttpGet]
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post(CreateUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpGet("TCKimlikNoDogrulama")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<bool>))]
    public async Task<IActionResult> Post(ConfirmCitizienshipCommand parameter)
    {
        ConfirmCitizienshipCommand command = new ConfirmCitizienshipCommand
        {
            CitizienshipId = parameter.CitizienshipId,
            Name = parameter.Name,
            Surname = parameter.Surname,
            Birthyear = parameter.Birthyear
        };

        return Ok(await Mediator.Send(command));
    }

    [Authorize]
    [HttpGet("GetAuthenticatedUser")]
    public async Task<IActionResult> Get()
    {
        return Ok(await Mediator.Send(new GetUserByIdQuery { }));
    }

    [Authorize]
    [HttpPut]
    public async Task<Response<string>> Put(UpdateUserCommand command)
    {
        return await Mediator.Send(command);
    }
}