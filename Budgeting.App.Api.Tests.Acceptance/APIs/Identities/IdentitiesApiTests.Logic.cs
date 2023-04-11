using Budgeting.App.Api.Tests.Acceptance.Models.IdentityRequests;
using Budgeting.App.Api.Tests.Acceptance.Models.IdentityResponses;
using Budgeting.App.Api.Tests.Acceptance.Models.Users;
using FluentAssertions;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Acceptance.APIs.Identities
{
    public partial class IdentitiesApiTests
    {
        [Fact]
        public async Task ShouldAuthenticatedUserAsync()
        {
            //given
            User randomUser = CreateRandomUser();
            User inputUser = randomUser;
            string randomPassword = GetRandomPassword();

            IdentityRequest inputIdentityRequest =
                CreateIdentityRequest(inputUser, randomPassword);

            IdentityResponse identityResponse =
                CreateIdentityResponse(inputUser);

            IEnumerable<Claim> expectedUserClaims =
                ParseClaimsFromJwt(identityResponse.Token);

            await this.budgetingAppApiBroker.PostUserAsync(inputUser, randomPassword);

            //when
            IdentityResponse actualIdentityResponse =
                await this.budgetingAppApiBroker.PostIdentityAsync(inputIdentityRequest);

            IEnumerable<Claim> actualUserClaims =
                ParseClaimsFromJwt(actualIdentityResponse.Token);

            //then
            actualUserClaims.Should().BeEquivalentTo(expectedUserClaims);
            await this.budgetingAppApiBroker.AddAuthenticationHeaderAsync(inputUser, randomPassword);
            await this.budgetingAppApiBroker.DeleteUserAsync(inputUser.Id);
            await this.budgetingAppApiBroker.RemoveAuthenticationHeaderAsync(inputUser);

        }
    }
}
