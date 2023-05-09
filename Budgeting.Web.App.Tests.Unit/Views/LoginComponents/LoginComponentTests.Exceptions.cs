using Budgeting.Web.App.Models.LoginViews;
using Budgeting.Web.App.Models.LoginViews.Exceptions;
using Budgeting.Web.App.Views.Components.LoginComponents;
using FluentAssertions;
using Moq;
using MudBlazor;
using System;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Views.LoginComponents
{
    public partial class LoginComponentTests
    {
        [Fact]
        public void ShouldRenderInnerValidationExceptionMessageIfErrorOccures()
        {
            //given
            var someLoginView = CreateRandomLoginView();
            var randomMessage = GetRandomString();
            var validationMessage = randomMessage;
            var innerValidationException = new Exception(validationMessage);

            var expectedLoginViewValidationException =
                new LoginViewValidationException(
                    innerException: innerValidationException,
                    data: innerValidationException.Data);

            this.loginViewServiceMock.Setup(service =>
                service.LoginAsync(It.IsAny<LoginView>()))
                         .ThrowsAsync(expectedLoginViewValidationException);

            //when
            this.renderLoginComponent = RenderComponent<LoginComponent>();

            renderLoginComponent.Instance.SubmitButton.Click();

            //then
            renderLoginComponent.Instance.EmailTextBox.IsDisabled
                .Should().BeFalse();

            renderLoginComponent.Instance.PasswordTextBox.IsDisabled
                .Should().BeFalse();

            this.loginViewServiceMock.Verify(service =>
                 service.LoginAsync(It.IsAny<LoginView>()),
                   Times.Once);

            this.toastBroker.Verify(broker =>
                broker.AddToast(
                    It.IsAny<string>(),
                    It.IsAny<Severity>()),
                       Times.Once,
                         expectedLoginViewValidationException.InnerException.Message);

            this.loginViewServiceMock.VerifyNoOtherCalls();
            this.toastBroker.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(LoginViewDependencyServiceException))]
        public void ShouldRenderOuterExceptionMessageIfDependencyOrServiceErrorOccured(
            Exception loginVewDependencyServiceException)
        {
            //given
            string expectedErrorMessage =
                loginVewDependencyServiceException.Message;

            this.loginViewServiceMock.Setup(service =>
                service.LoginAsync(It.IsAny<LoginView>()))
                   .ThrowsAsync(loginVewDependencyServiceException);

            //when
            this.renderLoginComponent = RenderComponent<LoginComponent>();

            this.renderLoginComponent.Instance.SubmitButton.Click();

            //then
            renderLoginComponent.Instance.EmailTextBox.IsDisabled
             .Should().BeFalse();

            renderLoginComponent.Instance.PasswordTextBox.IsDisabled
                .Should().BeFalse();

            this.loginViewServiceMock.Verify(service =>
                 service.LoginAsync(It.IsAny<LoginView>()),
                   Times.Once);

            this.toastBroker.Verify(broker =>
                broker.AddToast(
                    It.IsAny<string>(),
                    It.IsAny<Severity>()),
                       Times.Once,
                         expectedErrorMessage);

            this.loginViewServiceMock.VerifyNoOtherCalls();
            this.toastBroker.VerifyNoOtherCalls();
        }
    }
}
