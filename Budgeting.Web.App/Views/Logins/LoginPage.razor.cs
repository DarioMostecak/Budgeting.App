using Budgeting.Web.App.Models.LoginViews;
using Budgeting.Web.App.Models.LoginViews.Exceptions;
using Budgeting.Web.App.Services.Views.LoginViews;
using Budgeting.Web.App.Views.Bases;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Budgeting.Web.App.Views.Logins
{
    public partial class LoginPage : ComponentBase
    {

        [Inject]
        private ILoginViewService LoginViewService { get; set; }

        [Inject]
        private ISnackbar SnackbarService { get; set; }

        public TextBoxBase EmailTextBox { get; set; }
        public TextBoxBase PasswordTextBox { get; set; }
        public LoginView loginModel { get; set; }
        public ButtonBase? SubmitButton { get; set; }


        protected override void OnInitialized()
        {
            this.loginModel = new LoginView();
        }

        public async void LoginUserAsync()
        {
            try
            {
                await this.LoginViewService.LoginAsync(loginModel);
                NavigateToUserMenuPage();

                StateHasChanged();
            }
            catch (LoginViewValidationException loginViewValidationException)
            {
                this.SnackbarService.Add(
                    message: loginViewValidationException.Message,
                    severity: Severity.Error);
            }
            catch (LoginViewUnauthorizeException loginViewUnauthorizeException)
            {
                this.SnackbarService.Add(
                    message: loginViewUnauthorizeException.Message,
                    severity: Severity.Error);
            }
            catch (LoginViewDependencyException loginViewDependencyException)
            {
                this.SnackbarService.Add(
                    message: loginViewDependencyException.Message,
                    severity: Severity.Error);
            }
            catch (LoginViewServiceException loginViewServiceException)
            {
                this.SnackbarService.Add(
                    message: loginViewServiceException.Message,
                    severity: Severity.Error);
            }

        }

        private void NavigateToUserMenuPage() =>
            this.LoginViewService.NavigateTo("/usermenu");
    }
}
