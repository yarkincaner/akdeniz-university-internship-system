using Amazon.Auth.AccessControlPolicy;
using Internships.Core.Interfaces;
using Internships.Core.Settings;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Internships.Infrastructure.Services
{
    public class ExternalAccountService:IExternalAccountService
    {
        private readonly TokenSettings _tokenSettings;

        public ExternalAccountService(IOptions<TokenSettings> tokenSettings)
        {
            _tokenSettings = tokenSettings.Value;
        }
        //Create Jwt Token with email and internshipId
        public string GenerateToken(string employeeEmail, int internshipId)
        {
            var claims = new[]
            {
                new Claim("employee-mail",employeeEmail),
                new Claim("internshipId",internshipId.ToString())
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(
                issuer: _tokenSettings.Issuer,
                audience: _tokenSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),
                signingCredentials: signingCredentials);
            var origin = _tokenSettings.FrontendUrl;
            var extend = "/?token=";
            var stringToken = TokentoString(securityToken);
            var url = origin + extend + stringToken;
            return url;
        }
        //Encode jwt token and return ClaimsPrincipal
        private string TokentoString(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }  
        public int DecodeToken(string token)
        {
            //employee email kontrol edilecek
            //Employee email geri dönüşü olacak
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _tokenSettings.Issuer,
                ValidAudience = _tokenSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key)),
            };

            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                var internshipIdClaim = principal.FindFirst("internshipId");
                return int.Parse(internshipIdClaim.Value);
            }
            catch (SecurityTokenExpiredException)
            {
                // Token süresi dolmuşsa
                throw new Exception("Token has expired");
            }
            catch (SecurityTokenException)
            {
                // Token geçersizse
                throw new Exception("Invalid token");
            }
            catch (Exception)
            {
                // Diğer hatalar
                throw new Exception("An error occurred while validating the token");
            }
        }

    }
}
