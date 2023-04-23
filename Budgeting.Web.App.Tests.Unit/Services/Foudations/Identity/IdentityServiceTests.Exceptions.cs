using Budgeting.Web.App.Models.AuthenticationRequests;
using Budgeting.Web.App.Models.AuthenticationRequests.Exceptions;
using Budgeting.Web.App.Models.AuthenticationResults;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Identity
{
    public partial class IdentityServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyApiExceptions))]
        public async Task ShouldThowDependencyExceptionOnAuthenticateIfErrorOccuresAndLogItAsync(
            Exception httpResponseDependencyException)
        {
            //given
            AuthenticationRequest someRequest = CreateAuthenticationRequest();

            var failAuthenticationRequestDependencyException =
                new FailedAuthenticationRequestDependencyException(httpResponseDependencyException);

            var expectedAuthenticationRequestDependencyException =
                new AuthenticationRequestDependencyException(failAuthenticationRequestDependencyException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostLoginAsync(It.IsAny<AuthenticationRequest>()))
                        .ThrowsAsync(httpResponseDependencyException);

            //when
            ValueTask<AuthenticationResult> authenticateIdentityTask =
                this.identityService.AuthenticateIdentityAsync(someRequest);

            //then
            await Assert.ThrowsAsync<AuthenticationRequestDependencyException>(() =>
                authenticateIdentityTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuthenticationRequestDependencyException))),
                       Times.Once);

            this.apiBrokerMock.Verify(broker =>
                broker.PostLoginAsync(It.IsAny<AuthenticationRequest>()),
                   Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAuthenticateIfAuthenticationResultIsNullAndLogItAsync()
        {
            //given
            AuthenticationRequest someAuthenticationRequest = CreateAuthenticationRequest();
            AuthenticationResult nullAuthenticationResult = null;

            var nullAuthenticationResultException =
                new NullAuthenticationRequestException();

            var failAuthenticationRequestDependencyException =
                new FailedAuthenticationRequestDependencyException(nullAuthenticationResultException);

            var expectedAuthenticationResultDependencyException =
                new AuthenticationRequestDependencyException(failAuthenticationRequestDependencyException);

            this.apiBrokerMock.Setup(broker =>
                 broker.PostLoginAsync(someAuthenticationRequest))
                         .ReturnsAsync(nullAuthenticationResult);

            //when
            ValueTask<AuthenticationResult> authenticateIdentityTask =
                this.identityService.AuthenticateIdentityAsync(someAuthenticationRequest);

            //then
            await Assert.ThrowsAsync<AuthenticationRequestDependencyException>(() =>
                 authenticateIdentityTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAuthenticationResultDependencyException))),
                        Times.Once);

            this.apiBrokerMock.Verify(broker =>
                 broker.PostLoginAsync(It.IsAny<AuthenticationRequest>()),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAuthenticateIfErrorOccuresAndLogItAsync()
        {
            //given
            AuthenticationRequest someRequest = CreateAuthenticationRequest();
            var serviceException = new Exception();

            var failAuthenticationRequestServiceException =
                new FailedAuthenticationRequestServiceException(serviceException);

            var expectedAuthenticationRequestServiceException =
                new AuthenticationRequestServiceException(failAuthenticationRequestServiceException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostLoginAsync(It.IsAny<AuthenticationRequest>()))
                        .ThrowsAsync(serviceException);

            //when
            ValueTask<AuthenticationResult> authenticateIdentityTask =
                this.identityService.AuthenticateIdentityAsync(someRequest);

            //then
            await Assert.ThrowsAsync<AuthenticationRequestServiceException>(() =>
                authenticateIdentityTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAuthenticationRequestServiceException))),
                       Times.Once);

            this.apiBrokerMock.Verify(broker =>
                broker.PostLoginAsync(It.IsAny<AuthenticationRequest>()),
                   Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.apiBrokerMock.VerifyNoOtherCalls();
        }
    }
}
