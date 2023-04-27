using Budgeting.Web.App.Models.ContainerComponents;
using Budgeting.Web.App.Models.LoginViews;
using Budgeting.Web.App.Models.LoginViews.Exceptions;
using Budgeting.Web.App.Services.Views.LoginViews;
using Budgeting.Web.App.Views.Bases;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Budgeting.Web.App.Views.Components.LoginComponents
{
    public partial class LoginComponent
    {
        [Inject]
        private ILoginViewService LoginViewService { get; set; }

        [Inject]
        private ISnackbar SnackbarService { get; set; }

        public ComponentState State { get; set; }
        public TextBoxBase? EmailTextBox { get; set; }
        public TextBoxBase? PasswordTextBox { get; set; }
        public ButtonBase? SubmitButton { get; set; }
        public LoginView? loginModel { get; set; }


        protected override void OnInitialized()
        {
            this.State = ComponentState.Content;
            this.loginModel = new LoginView();
        }

        public async void LoginUserAsync()
        {
            try
            {
                ApplySubmitingStatus();
                await this.LoginViewService.LoginAsync(loginModel);
                NavigateToUserMenuPage();
            }
            catch (LoginViewValidationException loginViewValidationException)
            {
                ApplySubmisionFailed(
                    message: loginViewValidationException.Message,
                    severity: Severity.Warning);
            }
            catch (LoginViewUnauthorizeException loginViewUnauthorizeException)
            {

                ApplySubmisionFailed(
                    message: loginViewUnauthorizeException.Message,
                    severity: Severity.Warning);
            }
            catch (LoginViewDependencyException loginViewDependencyException)
            {

                ApplySubmisionFailed(
                    message: loginViewDependencyException.Message,
                    severity: Severity.Error);
            }
            catch (LoginViewServiceException loginViewServiceException)
            {

                ApplySubmisionFailed(
                    message: loginViewServiceException.Message,
                    severity: Severity.Error);
            }

        }

        private void NavigateToUserMenuPage() =>
            this.LoginViewService.NavigateTo("/usermenu");

        private void ApplySubmitingStatus()
        {
            this.loginModel.Email =
                this.EmailTextBox.Value;

            this.loginModel.Password =
                this.PasswordTextBox.Value;


            this.EmailTextBox.Disable();
            this.PasswordTextBox.Disable();
            this.SubmitButton.Disable();
        }

        private void ApplySubmisionFailed(string message, Severity severity)
        {
            this.SnackbarService.Add(
                    message: message,
                    severity: severity);

            this.EmailTextBox.Enable();
            this.PasswordTextBox.Enable();
            this.SubmitButton.Enable();
        }
    }
}

