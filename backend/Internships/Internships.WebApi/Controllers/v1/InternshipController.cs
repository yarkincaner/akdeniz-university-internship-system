using Internships.Core.Features.Internships.Commands.ApproveInternshipById;
using Internships.Core.Features.Internships.Commands.CreateInternship;
using Internships.Core.Features.Internships.Commands.CreateSpreadsheetFromInternship;
using Internships.Core.Features.Internships.Commands.DeleteInternshipById;
using Internships.Core.Features.Internships.Commands.UpdateInternship;
using Internships.Core.Features.Internships.Queries.GetAllInternships;
using Internships.Core.Features.Internships.Queries.GetInternshipById;
using Internships.Core.Features.Internships.Queries.GetInternshipsByStatus;
using Internships.Core.Features.Internships.Queries.GetInternshipsByUserId;
using Internships.Core.Parameters;
using Internships.Core.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internships.WebApi.Controllers.v1
{
    [Authorize]
    public class InternshipController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<IEnumerable<GetAllInternshipsViewModel>>))]
        public async Task<Response<IEnumerable<GetAllInternshipsViewModel>>> Get([FromQuery] GetAllInternshipsParameter filter)
        {
            var userClaims = User.Claims;
            var claimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
            var expectedClaimValue = "csestaj@ogr.akdeniz.edu.tr";
            var hasNameClaim = userClaims.Any(claim =>
                claim.Type == claimType && claim.Value == expectedClaimValue);
            if (!hasNameClaim) throw new UnauthorizedAccessException("");

            return await Mediator.Send(new GetAllInternshipsQuery()
            {
                SearchString = filter.SearchString,
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber,
            });
        }
        [HttpGet("UserID")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<IEnumerable<GetInternshipsByUserIdViewModel>>))]
        public async Task<PagedResponse<IEnumerable<GetInternshipsByUserIdViewModel>>> GetByUserId([FromQuery] GetAllInternshipsParameter filter)
        {
            return await Mediator.Send(new GetInternshipsByUserIdQuery
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber,
            });
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetInternshipByIdQuery { Id = id }));
        }

        // GET api/<controller>/Pending
        [HttpGet("Pending")]
        public async Task<Response<IEnumerable<GetAllInternshipsViewModel>>> Get()
        {
            //if (!Enum.TryParse<StatusName>(statusName, true, out StatusName status))
            //{
            //    return BadRequest("Invalid status name.");
            //}

            return await Mediator.Send(new GetInternshipsByStatusQuery { StatusName = "Pending" });
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(CreateInternshipCommandParameter parameter)
        {
            var claimUserId = User.Claims.FirstOrDefault(e => e.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            CreateInternshipCommand command = new CreateInternshipCommand
            {
                CompanyId = parameter.CompanyId,
                EmployeeId = parameter.EmployeeId,
                UserId = claimUserId,
                StartDate = parameter.StartDate,
                EndDate = parameter.EndDate,
                TotalDays = parameter.TotalDays,
                InsuranceType = parameter.InsuranceType,
            };
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateInternshipCommand command)
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
            var userClaims = User.Claims;
            var claimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
            var expectedClaimValue = "csestaj@ogr.akdeniz.edu.tr";
            var hasNameClaim = userClaims.Any(claim =>
                claim.Type == claimType && claim.Value == expectedClaimValue);
            if (!hasNameClaim) throw new UnauthorizedAccessException("");

            return Ok(await Mediator.Send(new DeleteInternshipByIdCommand { Id = id }));
        }

        [HttpPut("ApproveByInternshipCommittee")]
        public async Task<IActionResult> Put(ApproveInternshipByIdFromInternshipCommitteeCommand command)
        {
            var userClaims = User.Claims;
            var claimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
            var expectedClaimValue = "csestaj@ogr.akdeniz.edu.tr";
            var hasNameClaim = userClaims.Any(claim =>
                claim.Type == claimType && claim.Value == expectedClaimValue);
            if (!hasNameClaim) throw new UnauthorizedAccessException("");

            return Ok(await Mediator.Send(command));
        }

        [HttpPost("CreateSpreadsheet")]
        public async Task<IActionResult> Post([FromBody] List<int> internshipsIds)
        {
            var userClaims = User.Claims;
            var claimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
            var expectedClaimValue = "csestaj@ogr.akdeniz.edu.tr";
            var hasNameClaim = userClaims.Any(claim =>
                claim.Type == claimType && claim.Value == expectedClaimValue);
            if (!hasNameClaim) throw new UnauthorizedAccessException("");

            return Ok(await Mediator.Send(new CreateSpreadsheetFromInternshipCommand
            {
                InternshipIds = internshipsIds
            }));
        }
    }
}
