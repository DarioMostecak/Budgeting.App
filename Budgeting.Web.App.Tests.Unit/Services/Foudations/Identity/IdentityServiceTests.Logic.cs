using Budgeting.Web.App.Models.AuthenticationRequests;
using Budgeting.Web.App.Models.AuthenticationResults;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Identity
{
    public partial class IdentityServiceTests
    {
        [Fact]
        public async Task ShouldAuthenicateIdentityAsync()
        {
            //given
            AuthenticationRequest someAuthenticationRequest = CreateAuthenticationRequest();
            AuthenticationRequest inputAuthenticationRequest = someAuthenticationRequest;
            AuthenticationResult someAuthenticationResult = CreateAuthenticationResult();
            AuthenticationResult expectedAuthenticationResult = someAuthenticationResult.DeepClone();

            this.apiBrokerMock.Setup(broker =>
                broker.PostLoginAsync(It.IsAny<AuthenticationRequest>())).ReturnsAsync(someAuthenticationResult);

            //when
            AuthenticationResult actualAuthenticationResult =
                await this.identityService.AuthenticateIdentity(inputAuthenticationRequest);

            //then
            actualAuthenticationResult.Should().BeEquivalentTo(expectedAuthenticationResult);

            this.apiBrokerMock.Verify(broker =>
               broker.PostLoginAsync(It.IsAny<AuthenticationRequest>()),
                  Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }
    }
}
