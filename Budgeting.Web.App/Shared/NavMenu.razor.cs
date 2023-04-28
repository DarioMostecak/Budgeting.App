using Budgeting.Web.App.Brokers.AuthenticationProviders;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace Budgeting.Web.App.Shared
{
    public partial class NavMenu : ComponentBase
    {

        [Inject]
        public IAuthenticationProviderBroker authStateProvider { get; set; }

        public class UserInfo
        {
            public string UserName { get; set; }
            public string UserEmail { get; set; }
        }

        UserInfo userInfo = new UserInfo();

        bool _drawerOpen = true;

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }


        protected override async Task OnInitializedAsync()
        {

            var authenticationState = await authStateProvider.GetAuthenticationStateAsync();

            if (authenticationState.User.Identity.IsAuthenticated)
            {
                userInfo.UserName = authenticationState.User.Claims.FirstOrDefault(claim => claim.Type == "UserFullName").Value;
                userInfo.UserName = authenticationState.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name).Value;
            }
        }
    }
}
