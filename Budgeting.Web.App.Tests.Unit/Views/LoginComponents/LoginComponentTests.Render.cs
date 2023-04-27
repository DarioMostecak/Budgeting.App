using Budgeting.Web.App.Models.AuthenticationResults;
using Budgeting.Web.App.Models.ContainerComponents;
using Budgeting.Web.App.Models.LoginViews;
using Budgeting.Web.App.Views.Components.LoginComponents;
using FluentAssertions;
using Moq;
using System;
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
            initialLoginComponent.State
                .Should()
                  .Be(expectedComponentState);

            initialLoginComponent.EmailTextBox
                .Should()
                  .BeNull();

            initialLoginComponent.PasswordTextBox
                .Should()
                  .BeNull();

            initialLoginComponent.SubmitButton
                .Should()
                  .BeNull();

            initialLoginComponent.LoginView
                .Should()
                  .BeNull();
        }

        [Fact]
        public void ShouldRenderComponent()
        {
            //given
            ComponentState expectedComponentState =
                ComponentState.Content;

            string expectedEmailTextBoxLabel = "Email";
            string expectedEmailHelpText = "Enter email";
            string expectedPasswordTextBoxLabel = "Password";
            string expectedPasswordTextBoxHelperText = "Enter password";
            string expectedSubmitButtonLabel = "SUBMIT";

            //when
            this.renderLoginComponent =
                RenderComponent<LoginComponent>();

            //then
            this.renderLoginComponent.Instance.LoginView
                .Should()
                  .NotBeNull();

            this.renderLoginComponent.Instance.State
                .Should()
                  .Be(expectedComponentState);

            this.renderLoginComponent.Instance.EmailTextBox
                .Should()
                  .NotBeNull();

            this.renderLoginComponent.Instance.EmailTextBox.IsDisabled
                .Should()
                  .BeFalse();

            this.renderLoginComponent.Instance.EmailTextBox.Label
                .Should()
                  .Be(expectedEmailTextBoxLabel);

            this.renderLoginComponent.Instance.EmailTextBox.HelperText
                .Should()
                  .Be(expectedEmailHelpText);

            this.renderLoginComponent.Instance.PasswordTextBox
                .Should()
                  .NotBeNull();

            this.renderLoginComponent.Instance.PasswordTextBox.IsDisabled
                .Should()
                  .BeFalse();

            this.renderLoginComponent.Instance.PasswordTextBox.Label
                .Should()
                  .Be(expectedPasswordTextBoxLabel);

            this.renderLoginComponent.Instance.PasswordTextBox.HelperText
                .Should()
                 .Be(expectedPasswordTextBoxHelperText);

            this.renderLoginComponent.Instance.SubmitButton.Label
                .Should()
                  .Be(expectedSubmitButtonLabel);

            this.loginViewServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldDisableControlsBeforeLoginSubmissionCompletes()
        {
            //given
            AuthenticationResult someAuthenticationResult =
                new AuthenticationResult();

            this.loginViewServiceMock.Setup(service =>
                service.LoginAsync(It.IsAny<LoginView>()))
                  .ReturnsAsync(
                        value: someAuthenticationResult,
                        delay: TimeSpan.FromMilliseconds(500));

            //when
            this.renderLoginComponent =
                RenderComponent<LoginComponent>();

            this.renderLoginComponent.Instance.SubmitButton.Click();

            //then
            this.renderLoginComponent.Instance.EmailTextBox.IsDisabled
                .Should()
                  .BeTrue();

            this.renderLoginComponent.Instance.PasswordTextBox.IsDisabled
                .Should()
                  .BeTrue();

            this.renderLoginComponent.Instance.SubmitButton.IsDisabled
                .Should()
                  .BeTrue();

            this.loginViewServiceMock.Verify(service =>
                 service.LoginAsync(It.IsAny<LoginView>()),
                    Times.Once);

            this.loginViewServiceMock.VerifyNoOtherCalls();
        }
    }
}
