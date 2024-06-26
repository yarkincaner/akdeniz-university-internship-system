using Internships.Core.Settings;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Internships.Core.Interfaces
{
    public interface IExternalAccountService
    {
        string GenerateToken(string employeeEmail, int internshipId);
        int DecodeToken(string token);
        
    }
}
