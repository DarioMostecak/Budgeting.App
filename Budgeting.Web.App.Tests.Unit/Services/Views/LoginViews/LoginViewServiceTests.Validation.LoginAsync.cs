using Budgeting.Web.App.Models.AuthenticationResults;
using Budgeting.Web.App.Models.LoginViews;
using Budgeting.Web.App.Models.LoginViews.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Views.LoginViews
{
    public partial class LoginViewServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnLoginAsyncIfLoginViewIsNullAndLogItAsync()
        {
            //given
            LoginView invalidLoginView = null;

            var nullLoginViewException =
                new NullLoginViewException();

            var expectedLoginViewValidationException =
                new LoginViewValidationException(
                    innerException: nullLoginViewException,
                    data: nullLoginViewException.Data);

            //when
            ValueTask<AuthenticationResult> loginAsyncTask =
                this.loginViewService.LoginAsync(invalidLoginView);

            //then
            await Assert.ThrowsAsync<LoginViewValidationException>(() =>
                 loginAsyncTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLoginViewValidationException))),
                      Times.Once);

            this.identityServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.navigationBrokerMock.VerifyNoOtherCalls();
            this.authenticationProviderBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidLoginVewData))]
        public async Task ShouldThrowValidationExceptionOnLoginAsyncIfLoginViwIsInvalidAndLogItAsync(
            string invalidEmail,
            string invalidPassword)
        {
            //given
            var invalidLoginView = new LoginView
            {
                Email = invalidEmail,
                Password = invalidPassword
            };

            var invalidLogInViewException =
                new InvalidLoginViewException();

            invalidLogInViewException.AddData(
                key: nameof(LoginView.Email),
                values: "Email can't be white space or null and must be type of email format.");

            invalidLogInViewException.AddData(
                key: nameof(LoginView.Password),
                values: "Password is required and must be at least 8 character long.");

            var expectedLoginViewValidationException =
                new LoginViewValidationException(
                    innerException: invalidLogInViewException,
                    data: invalidLogInViewException.Data);

            //when
            ValueTask<AuthenticationResult> loginAsyncTask =
                this.loginViewService.LoginAsync(invalidLoginView);

            //then
            await Assert.ThrowsAsync<LoginViewValidationException>(() =>
                loginAsyncTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLoginViewValidationException))),
                       Times.Once);

            this.identityServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.navigationBrokerMock.VerifyNoOtherCalls();
            this.authenticationProviderBrokerMock.VerifyNoOtherCalls();
        }
    }
}
