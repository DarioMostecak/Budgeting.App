using Budgeting.Web.App.Models.ContainerComponents;
using Budgeting.Web.App.Views.Components.LoginComponents;
using FluentAssertions;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Views.LoginComponents
{
    public partial class LoginComponentTests
    {
        [Fact]
        public void ShouldInitializeComponent()
        {
            //given
            ComponentState expectedComponentState =
                ComponentState.Loading;

            //when
            var initialLoginComponent =
                new LoginComponent();

            //then
            initialLoginComponent.State.Should().Be(expectedComponentState);
            initialLoginComponent.EmailTextBox.Should().BeNull();
            initialLoginComponent.PasswordTextBox.Should().BeNull();
            initialLoginComponent.SubmitButton.Should().BeNull();
            initialLoginComponent.loginModel.Should().BeNull();
        }
    }
}
