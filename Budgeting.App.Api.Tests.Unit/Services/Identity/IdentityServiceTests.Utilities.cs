using Budgeting.App.Api.Models.IdentityResponses;
using Budgeting.App.Api.Models.Users;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Budgeting.App.Api.Tests.Unit.Services.Identity
{
    public partial class IdentityServiceTests
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

        private static SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity claims)
        {
            byte[] key = Encoding.ASCII.GetBytes("f5422e6cdfde4af3bf631c7dd1f80b97");

            return new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(2),
                Audience = "SwaggerUI",
                Issuer = "BudgetingAppApi",
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

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            claims.AddRange(keyValuePairs.Where(kvp => kvp.Key != "jti" && kvp.Key != "iat" && kvp.Key != "exp" && kvp.Key != "nbf")
                    .Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
