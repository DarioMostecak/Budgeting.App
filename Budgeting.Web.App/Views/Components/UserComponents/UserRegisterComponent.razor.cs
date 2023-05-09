using Budgeting.Web.App.Brokers.Toasts;
using Budgeting.Web.App.Models.ContainerComponents;
using Budgeting.Web.App.Models.UserViews;
using Budgeting.Web.App.Services.Views.UserViews;
using Budgeting.Web.App.Views.Bases;
using Microsoft.AspNetCore.Components;


namespace Budgeting.Web.App.Views.Components.UserComponents
{
    public partial class UserRegisterComponent : ComponentBase
    {
        [Inject]
        private IUserViewService UserViewService { get; set; }

        [Inject]
        private IToastBroker ToastBroker { get; set; }

        public ComponentState State { get; set; }
        public TextBoxBase? FirstNameTextBox { get; set; }
        public TextBoxBase? LastNameTextBox { get; set; }
        public TextBoxBase? EmailTextBox { get; set; }
        public TextBoxBase? PasswordTextBox { get; set; }
        public TextBoxBase? ConfirmPasswordTextBox { get; set; }
        public ButtonBase? SubmitButton { get; set; }
        public UserView? UserView { get; set; }

        protected override void OnInitialized()
        {
            this.State = ComponentState.Content;
            this.UserView = new UserView();

        }

        public async void RegisterUserAsync()
        {
            try
            {
                ApplySubmitingStatus();
                MapToUserView();

                await this.UserViewService.AddUserViewAsync(this.UserView);
            }
            catch (Exception exception)
            {

            }
        }

        private void ApplySubmitingStatus()
        {
            this.FirstNameTextBox.Disable();
            this.LastNameTextBox.Disable();
            this.EmailTextBox.Disable();
            this.PasswordTextBox.Disable();
            this.PasswordTextBox?.Disable();
            this.ConfirmPasswordTextBox?.Disable();
            this.SubmitButton.Disable();
        }

        private void MapToUserView()
        {
            this.UserView.FirstName = this.FirstNameTextBox.Value;
            this.UserView.LastName = this.LastNameTextBox.Value;
            this.UserView.Email = this.EmailTextBox.Value;
            this.UserView.Password = this.PasswordTextBox.Value;
            this.UserView.ConfirmPassword = this.ConfirmPasswordTextBox.Value;
        }
    }
}
