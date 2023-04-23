using Blazored.LocalStorage;
using Budgeting.Web.App.Models.AuthenticationResults;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace Budgeting.Web.App.Brokers.AuthenticationProviders
{
    public class AuthenticationProviderBroker : AuthenticationStateProvider, IAuthenticationProviderBroker
    {
        private readonly ILocalStorageService localStorageService;

        public AuthenticationProviderBroker(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userLocalStorageResult =
                await localStorageService.GetItemAsync<IEnumerable<Claim>>("UserClaims");

            if (userLocalStorageResult == null)
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

            var claimsPrincipal = new ClaimsPrincipal(
                    new ClaimsIdentity(userLocalStorageResult, "jwtAuth"));

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public async Task RegisterAuthenticationState(AuthenticationResult authenticationResult)
        {
            await localStorageService
                        .SetItemAsync("AuthenticationToken", authenticationResult);

            var claims = ParseClaimsFromJwt(authenticationResult.Token);

            await localStorageService.SetItemAsync("UserClaims", claims);

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task ClearAuthenticationState()
        {
            await localStorageService.ClearAsync();

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
