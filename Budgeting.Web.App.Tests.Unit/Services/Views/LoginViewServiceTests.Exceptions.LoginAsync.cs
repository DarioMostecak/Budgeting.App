using Budgeting.Web.App.Models.AuthenticationRequests;
using Budgeting.Web.App.Models.AuthenticationRequests.Exceptions;
using Budgeting.Web.App.Models.AuthenticationResults;
using Budgeting.Web.App.Models.LoginViews;
using Budgeting.Web.App.Models.LoginViews.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Views
{
    public partial class LoginViewServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnLoginAsyncIfDependencyExceptionOccuresAndLogItAsync()
        {
            //given
            LoginView someLoginView = CreateLoginView();

            var exception = new Exception();

            var authenticationRequestValidationException =
                new AuthenticationRequestDependencyException(exception);

            var failLoginViewDependencyException =
                new FailedLoginVewDependencyException(authenticationRequestValidationException);

            var expectedLoginViewDependencyException =
                new LoginViewDependencyException(failLoginViewDependencyException);

            this.identityServiceMock.Setup(service =>
                service.AuthenticateIdentityAsync(It.IsAny<AuthenticationRequest>()))
                         .ThrowsAsync(authenticationRequestValidationException);

            //when
            ValueTask<AuthenticationResult> loginAsyncTask =
                this.loginViewService.LoginAsync(someLoginView);

            //then
            await Assert.ThrowsAsync<LoginViewDependencyException>(() =>
                 loginAsyncTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedLoginViewDependencyException))),
                       Times.Once);

            this.identityServiceMock.Verify(service =>
                 service.AuthenticateIdentityAsync(It.IsAny<AuthenticationRequest>()),
                    Times.Once);

            this.identityServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.navigationBrokerMock.VerifyNoOtherCalls();
            this.authenticationProviderBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(IdentityServiceExceptions))]
        public async Task ShouldThrowServiceExceptionOnLoginIfServiceExceptionOccuresAndLogItAsync(
            Exception serviceException)
        {
            //given
            LoginView someLoginView = CreateLoginView();

            var failedLoginViewServiceException =
                new FailedLoginViewServiceException(serviceException);

            var expectedLoginViewServiceException =
                new LoginViewServiceException(failedLoginViewServiceException);

            this.identityServiceMock.Setup(service =>
                 service.AuthenticateIdentityAsync(It.IsAny<AuthenticationRequest>()))
                          .ThrowsAsync(serviceException);

            //when
            ValueTask<AuthenticationResult> loginAsyncTask =
                this.loginViewService.LoginAsync(someLoginView);

            //then
            await Assert.ThrowsAsync<LoginViewServiceException>(() =>
                 loginAsyncTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedLoginViewServiceException))),
                        Times.Once);

            this.identityServiceMock.Verify(service =>
                 service.AuthenticateIdentityAsync(It.IsAny<AuthenticationRequest>()),
                    Times.Once);

            this.identityServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.navigationBrokerMock.VerifyNoOtherCalls();
            this.authenticationProviderBrokerMock.VerifyNoOtherCalls();
        }
    }
}
