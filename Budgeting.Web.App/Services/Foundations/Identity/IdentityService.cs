using Budgeting.Web.App.Brokers.Apis;
using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Models.AuthenticationRequests;
using Budgeting.Web.App.Models.AuthenticationResults;
using Budgeting.Web.App.Services.Foundations.Identity;

namespace Budgeting.Web.App.Services.Foundations
{
    public partial class IdentityService : IIdentityService
    {
        private readonly IApiBroker apiBroker;
        private readonly ILoggingBroker loggingBroker;

        public IdentityService(
            IApiBroker apiBroker,
            ILoggingBroker loggingBroker)
        {
            this.apiBroker = apiBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<AuthenticationResult> AuthenticateIdentityAsync(AuthenticationRequest authenticationRequest) =>
        TryCatch(async () =>
        {
            ValidateAuthenticationRequest(authenticationRequest);

            var authenticationResult =
                 await this.apiBroker.PostLoginAsync(authenticationRequest);

            ValidateAuthenticationResultIsNull(authenticationResult);

            return authenticationResult;
        });
    }
}
