using Budgeting.App.Api.Models.IdentityRequests;
using Budgeting.App.Api.Models.IdentityResponses;

namespace Budgeting.App.Api.Services.Identity
{
    public interface IIdentityService
    {
        ValueTask<IdentityResponse> AuthenticateUserAsync(IdentityRequest identityRequest);
    }
}
