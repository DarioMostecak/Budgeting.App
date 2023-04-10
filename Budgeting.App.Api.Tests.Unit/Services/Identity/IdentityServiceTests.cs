using Budgeting.App.Api.Brokers.Loggings;
using Budgeting.App.Api.Brokers.UserManagment;
using Budgeting.App.Api.Models.ExceptionModels;
using Budgeting.App.Api.Models.IdentityRequests;
using Budgeting.App.Api.Models.IdentityResponses;
using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Options;
using Budgeting.App.Api.Services.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Budgeting.App.Api.Tests.Unit.Services.Identity
{
    public partial class IdentityServiceTests
    {
        private readonly Mock<IUserManagerBroker> userManagerBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IOptions<JwtSettings>> jwtOptionsMock;
        private readonly IIdentityService identityService;


        public IdentityServiceTests()
        {
            this.userManagerBrokerMock = new Mock<IUserManagerBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.jwtOptionsMock = new Mock<IOptions<JwtSettings>>();
            this.jwtOptionsMock.Setup(jwt => jwt.Value).Returns(GetJwtSettings());

            this.identityService = new IdentityService(
                userManagerBroker: this.userManagerBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                jwtOptions: this.jwtOptionsMock.Object);
        }

        private static MongoException GetMongoException() =>
            (MongoException)FormatterServices.GetUninitializedObject(typeof(MongoException));

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static Expression<Func<Exception, bool>> SameValidationExceptionAs(Exception expectedException)
        {
            return actualException =>
            actualException.Message == expectedException.Message
            && (actualException.InnerException as ExceptionModel).DataEquals(expectedException.InnerException.Data);

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

        private IdentityRequest CreateRandomIdentityRequest() =>
            new IdentityRequest
            {
                Email = "person@mail.com",
                Password = "22222222"
            };

        private static User CreateRandomUser()
        {
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "Travis@mail.com",
                FirstName = "Travis",
                LastName = "Kongo",
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow.AddDays(10),
            };

            return newUser;
        }

        private static IEnumerable<object[]> InvalidIdentityRequestData() =>
            new List<object[]>
            {
                new object[] { null, null },
                new object[] {" ", " "},
                new object[] {"xcsdfascas", null}
            };

        private static JwtSettings GetJwtSettings() =>
            new JwtSettings
            {
                SigningKey = "f5422e6cdfde4af3bf631c7dd1f80b97",
                Audiences = new string[] { "SwaggerUI" },
                Issuer = "BudgetingAppApi"
            };

        #region Token Handler
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
        #endregion
    }
}
