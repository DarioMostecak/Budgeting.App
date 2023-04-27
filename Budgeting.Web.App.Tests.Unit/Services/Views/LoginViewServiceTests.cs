using Budgeting.Web.App.Brokers.AuthenticationProviders;
using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Brokers.Navigations;
using Budgeting.Web.App.Models.AuthenticationRequests.Exceptions;
using Budgeting.Web.App.Models.AuthenticationResults;
using Budgeting.Web.App.Models.ExceptionModels;
using Budgeting.Web.App.Models.LoginViews;
using Budgeting.Web.App.Services.Foundations.Identity;
using Budgeting.Web.App.Services.Views.LoginViews;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Views
{
    public partial class LoginViewServiceTests
    {
        private readonly Mock<IIdentityService> identityServiceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<INavigationBroker> navigationBrokerMock;
        private readonly Mock<IAuthenticationProviderBroker> authenticationProviderBrokerMock;
        private readonly ILoginViewService loginViewService;

        public LoginViewServiceTests()
        {
            this.identityServiceMock = new Mock<IIdentityService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.navigationBrokerMock = new Mock<INavigationBroker>();
            this.authenticationProviderBrokerMock = new Mock<IAuthenticationProviderBroker>();

            this.loginViewService = new LoginViewService(
                identityService: this.identityServiceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                navigationBroker: this.navigationBrokerMock.Object,
                authenticationProviderBroker: this.authenticationProviderBrokerMock.Object);
        }

        private static Expression<Func<ExceptionModel, bool>> SameExceptionAs(ExceptionModel expectedException)
        {
            return actualException => actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message
                && (actualException.InnerException as ExceptionModel).DataEquals(expectedException.InnerException.Data);
        }

        private static LoginView CreateLoginView() =>
            new LoginView
            {
                Email = "dario@gmail.com",
                Password = "11111111"
            };

        private static TheoryData IdentityServiceExceptions()
        {
            var exception = new Exception();

            var authenticationRequestServiceException =
                new AuthenticationRequestServiceException(exception);

            return new TheoryData<Exception>
            {
                authenticationRequestServiceException,
                exception
            };
        }

        public static IEnumerable<object[]> InvalidLoginVewData() =>
            new List<object[]>
            {
                new object[] { null, null },
                new object[] { string.Empty, string.Empty },
                new object[] { "  ", "  "},
                new object[] { " dariomail ", "111111"},
            };


        private AuthenticationResult CreateAuthenticationResult()
        {
            var authenticationResult = new AuthenticationResult();
            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "dario@mail.com"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, "dario@mail.com"),
                new Claim("UserId", Guid.NewGuid().ToString()),
                new Claim("UserFullName", "Dario Mostecak")
            });

            var token = CreateSecurityToken(claimsIdentity);
            authenticationResult.Token = WriteToken(token);

            return authenticationResult;
        }



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
