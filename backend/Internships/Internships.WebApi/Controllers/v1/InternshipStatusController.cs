using Internships.Core.Features.InternshipStatuses.Queries.GetInternshipStatusesByInternshipId;
using Internships.Core.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internships.WebApi.Controllers.v1
{
    [Authorize]
    public class InternshipStatusController : BaseApiController
    {
        [HttpGet("Internship/{internshipId}")]
        public async Task<Response<IEnumerable<GetInternshipStatusesByInternshipIdViewModel>>> GetInternshipStatusesByInternshipId(int internshipId)
        {
            return await Mediator.Send(new GetInternshipStatusesByInternshipIdQuery
            {
                InternshipId = internshipId
            });
        }
    }
}
