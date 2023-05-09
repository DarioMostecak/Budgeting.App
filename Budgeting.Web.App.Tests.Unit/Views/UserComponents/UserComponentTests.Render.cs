using Budgeting.Web.App.Models.ContainerComponents;
using Budgeting.Web.App.Models.UserViews;
using Budgeting.Web.App.Views.Components.UserComponents;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Views.UserComponents
{
    public partial class UserComponentTests
    {
        [Fact]
        public void ShouldInitializeComponent()
        {
            //given
            ComponentState expectedComponentState =
                ComponentState.Loading;

            //when
            var initialUserComponent =
                new UserRegisterComponent();

            //then
            initialUserComponent.State.Should().Be(expectedComponentState);
            initialUserComponent.FirstNameTextBox.Should().BeNull();
            initialUserComponent.LastNameTextBox.Should().BeNull();
            initialUserComponent.EmailTextBox.Should().BeNull();
            initialUserComponent.PasswordTextBox.Should().BeNull();
            initialUserComponent.ConfirmPasswordTextBox.Should().BeNull();
            initialUserComponent.UserView.Should().BeNull();
        }

        [Fact]
        public void ShouldRenderComponent()
        {
            //given
            ComponentState expectedComponentState =
                ComponentState.Content;

            string expectedFirstNameTextBoxLabel = "First name";
            string expectedLastNameTextBoxLabel = "Last name";
            string expectedEmailTextBoxLabel = "Email";
            string expectedPasswordTextBoxLabel = "Password";
            string expextedConfirmPasswordLabel = "Confirm password";
            string expectedSubmitButtonLabel = "SUBMIT";

            //when
            this.renderUserRegisterComponent =
                RenderComponent<UserRegisterComponent>();

            //then
            this.renderUserRegisterComponent.Instance.State.Should().Be(expectedComponentState);
            this.renderUserRegisterComponent.Instance.FirstNameTextBox.Should().NotBeNull();

            this.renderUserRegisterComponent.Instance.FirstNameTextBox.Label
                .Should()
                  .Be(expectedFirstNameTextBoxLabel);

            this.renderUserRegisterComponent.Instance.LastNameTextBox.Should().NotBeNull();

            this.renderUserRegisterComponent.Instance.LastNameTextBox.Label
                .Should()
                  .Be(expectedLastNameTextBoxLabel);

            this.renderUserRegisterComponent.Instance.EmailTextBox.Should().NotBeNull();

            this.renderUserRegisterComponent.Instance.EmailTextBox.Label
                .Should()
                  .Be(expectedEmailTextBoxLabel);

            this.renderUserRegisterComponent.Instance.PasswordTextBox.Should().NotBeNull();

            this.renderUserRegisterComponent.Instance.PasswordTextBox.Label
                .Should()
                  .Be(expectedPasswordTextBoxLabel);

            this.renderUserRegisterComponent.Instance.ConfirmPasswordTextBox.Should().NotBeNull();

            this.renderUserRegisterComponent.Instance.ConfirmPasswordTextBox.Label
                .Should()
                  .Be(expextedConfirmPasswordLabel);

            this.renderUserRegisterComponent.Instance.SubmitButton.Should().NotBeNull();

            this.renderUserRegisterComponent.Instance.SubmitButton.Label
                .Should()
                  .Be(expectedSubmitButtonLabel);

            this.renderUserRegisterComponent.Instance.UserView.Should().NotBeNull();

            this.userViewServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldDisableControlsBeforeUserSubmissionComplete()
        {
            //given
            UserView someUserView = new UserView();

            this.userViewServiceMock.Setup(service =>
                service.AddUserViewAsync(It.IsAny<UserView>()))
                  .ReturnsAsync(
                        value: someUserView,
                        delay: TimeSpan.FromMilliseconds(500));

            //when
            this.renderUserRegisterComponent =
                RenderComponent<UserRegisterComponent>();

            this.renderUserRegisterComponent.Instance.SubmitButton.Click();

            //then
            this.renderUserRegisterComponent.Instance.FirstNameTextBox.IsDisabled.Should().BeTrue();
            this.renderUserRegisterComponent.Instance.LastNameTextBox.IsDisabled.Should().BeTrue();
            this.renderUserRegisterComponent.Instance.EmailTextBox.IsDisabled.Should().BeTrue();
            this.renderUserRegisterComponent.Instance.PasswordTextBox.IsDisabled.Should().BeTrue();
            this.renderUserRegisterComponent.Instance.ConfirmPasswordTextBox.IsDisabled.Should().BeTrue();
            this.renderUserRegisterComponent.Instance.SubmitButton.IsDisabled.Should().BeTrue();

            this.userViewServiceMock.Verify(service =>
                service.AddUserViewAsync(It.IsAny<UserView>()),
                   Times.Once);

            this.userViewServiceMock.VerifyNoOtherCalls();
            this.toastBroker.VerifyNoOtherCalls();
        }
    }
}
