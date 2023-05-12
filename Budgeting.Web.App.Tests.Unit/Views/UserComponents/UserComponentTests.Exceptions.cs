using Budgeting.Web.App.Models.UserViews;
using Budgeting.Web.App.Views.Components.UserComponents;
using FluentAssertions;
using Moq;
using MudBlazor;
using System;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Views.UserComponents
{
    public partial class UserComponentTests
    {
        [Theory]
        [MemberData(nameof(UserViewValidationExceptions))]
        public void ShouldRenderInnerExceptionMessageIfValidationErrorOccured(
            Exception userViewValidationException)
        {
            //given
            string expectedErrorMessage =
                userViewValidationException.Message;

            this.userViewServiceMock.Setup(service =>
                service.AddUserViewAsync(It.IsAny<UserView>()))
                         .ThrowsAsync(userViewValidationException);

            //when
            this.renderUserRegisterComponent =
                RenderComponent<UserRegisterComponent>();

            renderUserRegisterComponent.Instance.SubmitButton.Click();

            //then
            renderUserRegisterComponent.Instance.FirstNameTextBox.IsDisabled
                .Should().BeFalse();

            renderUserRegisterComponent.Instance.LastNameTextBox.IsDisabled
                .Should().BeFalse();

            renderUserRegisterComponent.Instance.EmailTextBox.IsDisabled
                .Should().BeFalse();

            renderUserRegisterComponent.Instance.PasswordTextBox.IsDisabled
                .Should().BeFalse();

            renderUserRegisterComponent.Instance.ConfirmPasswordTextBox.IsDisabled
                .Should().BeFalse();

            this.userViewServiceMock.Verify(service =>
                 service.AddUserViewAsync(It.IsAny<UserView>()),
                   Times.Once);

            this.toastBroker.Verify(broker =>
                broker.AddToast(
                    It.IsAny<string>(),
                    It.IsAny<Severity>()),
                       Times.Once,
                         expectedErrorMessage);

            this.userViewServiceMock.VerifyNoOtherCalls();
            this.toastBroker.VerifyNoOtherCalls();
        }
    }
}
