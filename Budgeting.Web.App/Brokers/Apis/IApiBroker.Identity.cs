using Budgeting.Web.App.Models.AuthenticationRequests;
using Budgeting.Web.App.Models.AuthenticationResults;

namespace Budgeting.Web.App.Brokers.Apis
{
    public partial interface IApiBroker
    {
        ValueTask<AuthenticationResult> PostLoginAsync(AuthenticationRequest AuthenticationRequest);
    }
}
