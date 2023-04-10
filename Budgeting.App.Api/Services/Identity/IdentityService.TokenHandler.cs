using Budgeting.App.Api.Models.IdentityResponses;
using Budgeting.App.Api.Models.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Budgeting.App.Api.Services.Identity
{
    public partial class IdentityService
    {
        private JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();

        private SecurityToken CreateSecurityToken(ClaimsIdentity identity)
        {
            var tokenDescriptor = GetTokenDescriptor(identity);

            return TokenHandler.CreateToken(tokenDescriptor);
        }

        private string WriteToken(SecurityToken token)
        {
            return TokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity claims)
        {
            byte[] key = Encoding.ASCII.GetBytes(this.jwtSettings.SigningKey);

            return new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(2),
                Audience = this.jwtSettings.Audiences[0],
                Issuer = this.jwtSettings.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key)
                   , SecurityAlgorithms.HmacSha256Signature)

            };
        }

        private IdentityResponse CreateIdentityResponse(User user)
        {
            var identityResponse = new IdentityResponse();
            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("UserId", user.Id.ToString()),
                new Claim("UserFullName", user.FirstName + " " + user.LastName)
            });

            var token = CreateSecurityToken(claimsIdentity);
            identityResponse.Token = WriteToken(token);

            return identityResponse;
        }
    }
}
