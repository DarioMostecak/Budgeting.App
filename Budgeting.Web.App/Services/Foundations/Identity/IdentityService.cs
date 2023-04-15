using Budgeting.Web.App.Brokers.Apis;
using Budgeting.Web.App.Brokers.DateTimes;
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
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.apiBroker = apiBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<AuthenticationResult> AuthenticateIdentity(AuthenticationRequest authenticationRequest) =>
        TryCatch(async () =>
        {
            return new AuthenticationResult();
        });
    }
}
