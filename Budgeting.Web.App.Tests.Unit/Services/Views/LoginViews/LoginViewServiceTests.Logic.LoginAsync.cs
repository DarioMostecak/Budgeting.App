using Budgeting.Web.App.Models.AuthenticationRequests;
using Budgeting.Web.App.Models.AuthenticationResults;
using Budgeting.Web.App.Models.LoginViews;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Views.LoginViews
{
    public partial class LoginViewServiceTests
    {
        [Fact]
        public async Task ShouldAuthenticateAsync()
        {
            //given
            LoginView someLoginView = CreateLoginView();
            LoginView inputLoginView = someLoginView;

            AuthenticationResult someAuthenticationResult =
                CreateAuthenticationResult();

            AuthenticationResult returnigAuthenticationResult =
                someAuthenticationResult;

            AuthenticationResult expectedAuthenticationResult =
                someAuthenticationResult.DeepClone();

            this.identityServiceMock.Setup(service =>
                service.AuthenticateIdentityAsync(It.IsAny<AuthenticationRequest>()))
                          .ReturnsAsync(returnigAuthenticationResult);

            //when
            AuthenticationResult actualAuthenticationResult =
                await this.loginViewService.LoginAsync(inputLoginView);

            //then
            actualAuthenticationResult.Should().BeEquivalentTo(expectedAuthenticationResult);

            this.identityServiceMock.Verify(service =>
                 service.AuthenticateIdentityAsync(It.IsAny<AuthenticationRequest>()),
                    Times.Once);

            this.authenticationProviderBrokerMock.Verify(broker =>
                broker.RegisterAuthenticationState(It.IsAny<AuthenticationResult>()),
                   Times.Once);

            this.identityServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.navigationBrokerMock.VerifyNoOtherCalls();
            this.authenticationProviderBrokerMock.VerifyNoOtherCalls();

        }
    }
}
