using Internships.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Internships.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetaController : BaseApiController
    {
        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Program).Assembly;

            var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {lastUpdate}");
        } 
    }
}
