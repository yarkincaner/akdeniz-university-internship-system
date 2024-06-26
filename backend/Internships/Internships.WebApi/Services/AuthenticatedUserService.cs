using Internships.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Internships.WebApi.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(e => e.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
            Email = httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
            Name = httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value;
            Surname = httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")?.Value;

        }

        public string UserId { get; }
        public string Email { get; }
        public string Name { get; }
        public string Surname { get; }
    }
}
