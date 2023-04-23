using Budgeting.Web.App.Brokers.AuthenticationProviders;
using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Brokers.NavigationBroker;
using Budgeting.Web.App.Models.AuthenticationRequests;
using Budgeting.Web.App.Models.AuthenticationResults;
using Budgeting.Web.App.Models.LoginViews;
using Budgeting.Web.App.Services.Foundations.Identity;

namespace Budgeting.Web.App.Services.Views.LoginViews
{
    public partial class LoginViewService : ILoginViewService
    {
        private readonly IIdentityService identityService;
        private readonly ILoggingBroker loggingBroker;
        private readonly INavigationBroker navigationBroker;
        private readonly IAuthenticationProviderBroker authenticationProviderBroker;

        public LoginViewService(
            IIdentityService identityService,
            ILoggingBroker loggingBroker,
            INavigationBroker navigationBroker,
            IAuthenticationProviderBroker authenticationProviderBroker)
        {
            this.identityService = identityService;
            this.loggingBroker = loggingBroker;
            this.navigationBroker = navigationBroker;
            this.authenticationProviderBroker = authenticationProviderBroker;
        }

        public ValueTask<AuthenticationResult> LoginAsync(LoginView loginView) =>
        TryCatch(async () =>
        {
            ValidateLoginView(loginView);

            AuthenticationRequest authenticationRequest =
                  MapToAuthenticationRequest(loginView);

            AuthenticationResult authenticationResult =
                  await this.identityService.AuthenticateIdentityAsync(authenticationRequest);

            await authenticationProviderBroker.RegisterAuthenticationState(authenticationResult);

            return authenticationResult;
        });

        public void NavigateTo(string route) =>
        TryCatch(() =>
        {
            ValidateRoute(route);
            this.navigationBroker.NavigateTo(route);
        });

        private AuthenticationRequest MapToAuthenticationRequest(LoginView loginView) =>
            new AuthenticationRequest
            {
                Email = loginView.Email,
                Password = loginView.Password
            };

    }
}
