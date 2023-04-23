using Budgeting.Web.App.Models.AuthenticationRequests;
using Budgeting.Web.App.Models.AuthenticationResults;

namespace Budgeting.Web.App.Services.Foundations.Identity
{
    public interface IIdentityService
    {
        ValueTask<AuthenticationResult> AuthenticateIdentityAsync(AuthenticationRequest authenticationRequest);
    }
}
