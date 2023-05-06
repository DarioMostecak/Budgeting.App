using Budgeting.Web.App.Models.ContainerComponents;
using Budgeting.Web.App.Models.UserViews;
using Budgeting.Web.App.Views.Bases;
using Microsoft.AspNetCore.Components;


namespace Budgeting.Web.App.Views.Components.UserComponents
{
    public partial class UserRegisterComponent : ComponentBase
    {
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

        }

    }
}
