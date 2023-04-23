using Budgeting.Web.App.Models.AuthenticationResults;
using Microsoft.AspNetCore.Components.Authorization;

namespace Budgeting.Web.App.Brokers.AuthenticationProviders
{
    public interface IAuthenticationProviderBroker
    {
        Task<AuthenticationState> GetAuthenticationStateAsync();
        Task RegisterAuthenticationState(AuthenticationResult authenticationResult);
        Task ClearAuthenticationState();
    }
}
