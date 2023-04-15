using Blazored.LocalStorage;
using Budgeting.Web.App.Models.AuthenticationResults;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace Budgeting.Web.App.AuthenticationProviders
{
    public class CustomAuthenticationProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService localStorageService;

        public CustomAuthenticationProvider(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userLocalStorageResult =
                await this.localStorageService.GetItemAsync<IEnumerable<Claim>>("UserClaims");

            if (userLocalStorageResult == null)
                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

            var claimsPrincipal = new ClaimsPrincipal(
                    new ClaimsIdentity(userLocalStorageResult, "jwtAuth"));

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public async Task RegisterAuthenticationState(AuthenticationResult authenticationResult)
        {
            await this.localStorageService
                        .SetItemAsync<AuthenticationResult>("AuthenticationToken", authenticationResult);

            var claims = ParseClaimsFromJwt(authenticationResult.Token);

            await this.localStorageService.SetItemAsync<IEnumerable<Claim>>("UserClaims", claims);
        }

        public async Task ClearAuthenticationState()
        {
            await this.localStorageService.ClearAsync();
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

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
