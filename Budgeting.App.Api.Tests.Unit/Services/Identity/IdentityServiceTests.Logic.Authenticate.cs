using Budgeting.App.Api.Models.IdentityRequests;
using Budgeting.App.Api.Models.IdentityResponses;
using Budgeting.App.Api.Models.Users;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Identity
{
    public partial class IdentityServiceTests
    {
        [Fact]
        public async Task ShouldAuthenticateIdentityRequestAsync()
        {
            //given
            User user = CreateRandomUser();
            IdentityRequest identityRequest = CreateRandomIdentityRequest();
            IdentityResponse expectedIdentityResponse = CreateIdentityResponse(user);
            IEnumerable<Claim> expectedIdentityResponseClaims = ParseClaimsFromJwt(expectedIdentityResponse.Token);

            this.userManagerBrokerMock.Setup(broker =>
               broker.SelectUserByEmailAsync(It.IsAny<string>()))
                  .ReturnsAsync(user);

            this.userManagerBrokerMock.Setup(broker =>
               broker.ConfirmUserByPasswordAsync(
                   It.IsAny<User>(),
                   It.IsAny<string>()))
                     .ReturnsAsync(true);

            //when
            IdentityResponse actualIdentityResponse =
                await this.identityService.AuthenticateUserAsync(identityRequest);

            //test clamis
            var actualIdentityResponseClaims = ParseClaimsFromJwt(actualIdentityResponse.Token);


            //then
            actualIdentityResponseClaims.Should().BeEquivalentTo(expectedIdentityResponseClaims);

            this.userManagerBrokerMock.Verify(manager =>
                manager.SelectUserByEmailAsync(It.IsAny<string>()),
                  Times.Once);

            this.userManagerBrokerMock.Verify(manager =>
                manager.ConfirmUserByPasswordAsync(It.IsAny<User>(), It.IsAny<string>()),
                  Times.Once);

            this.userManagerBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
