using Budgeting.App.Api.Tests.Acceptance.Models.IdentityRequests;
using Budgeting.App.Api.Tests.Acceptance.Models.IdentityResponses;
using System.Threading.Tasks;

namespace Budgeting.App.Api.Tests.Acceptance.Brokers
{
    public partial class BudgetingAppApiBroker
    {
        private const string IdentityRelativeUrl = "api/v1/Identities";

        public async ValueTask<IdentityResponse> PostIdentityAsync(IdentityRequest identityRequest) =>
            await this.PostContentAsync<IdentityRequest, IdentityResponse>(IdentityRelativeUrl, identityRequest);
    }
}
