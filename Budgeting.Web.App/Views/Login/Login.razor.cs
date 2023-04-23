using Budgeting.Web.App.Models.LoginViews;
using Budgeting.Web.App.Models.LoginViews.Exceptions;
using Budgeting.Web.App.Services.Views.LoginViews;
using Budgeting.Web.App.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Budgeting.Web.App.Views.Login
{
    public partial class Login : ComponentBase
    {

        [Inject] private ILoginViewService LoginViewService { get; set; }
        public MudMessageBox? MudMessageBox { get; set; }
        [Inject] private IDialogService DialogService { get; set; }

        public bool success;
        public LoginView loginModel { get; set; }

        string state = "Message box hasn't been opened yet";

        protected override void OnInitialized()
        {
            this.loginModel = new LoginView();
        }

        public async Task LoginUserAsync()
        {
            try
            {
                await this.LoginViewService.LoginAsync(loginModel);
                this.LoginViewService.NavigateTo("/usermenu");

                StateHasChanged();
            }
            catch (LoginViewValidationException loginViewValidationException)
            {
                ShowErrorMessageBox(loginViewValidationException.Message);
            }
            catch (LoginViewDependencyException loginViewDependencyException)
            {
                ShowErrorMessageBox(loginViewDependencyException.Message);
            }
            catch (LoginViewServiceException loginViewServiceException)
            {
                ShowErrorMessageBox(loginViewServiceException.Message);
            }
            catch (Exception exception)
            {
                ShowErrorMessageBox("Unexpected error occored.\nContact support.");
            }
        }

        private void ShowErrorMessageBox(string errorMessage)
        {
            var parameters = new DialogParameters();

            parameters.Add("ContentText", errorMessage);
            this.DialogService.Show<ErrorMessageDialog>("Error!", parameters);
        }

    }
}
