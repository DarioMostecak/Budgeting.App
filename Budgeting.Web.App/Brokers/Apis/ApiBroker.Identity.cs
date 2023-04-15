using Budgeting.Web.App.Models.AuthenticationRequests;
using Budgeting.Web.App.Models.AuthenticationResults;

namespace Budgeting.Web.App.Brokers.Apis
{
    public partial class ApiBroker
    {
        private const string IdentityRelativeUrl = "";

        public async ValueTask<AuthenticationResult> PostLoginAsync(AuthenticationRequest authenticationRequest) =>
            await this.PostContentAsync<AuthenticationRequest, AuthenticationResult>(IdentityRelativeUrl, authenticationRequest);
    }
}
