using Budgeting.App.Api.Models.IdentityRequests;
using Budgeting.App.Api.Models.IdentityRequests.Exceptions;
using Budgeting.App.Api.Models.IdentityResponses;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Identity
{
    public partial class IdentityServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAuthenticateIfMongoExceptionOccuresAndLogItAsync()
        {
            //given
            IdentityRequest someRequest = CreateRandomIdentityRequest();
            MongoException mongoException = GetMongoException();

            var failIdentityResponseServiceException =
                new FailIdentityRequestServiceException(mongoException);

            var expectedIdentityRequestDependencyException =
                new IdentityRequestDependencyException(failIdentityResponseServiceException);

            this.userManagerBrokerMock.Setup(broker =>
                broker.SelectUserByEmailAsync(someRequest.Email))
                   .ThrowsAsync(mongoException);

            //when
            ValueTask<IdentityResponse> authenticationTask =
                this.identityService.AuthenticateUserAsync(someRequest);

            //then
            await Assert.ThrowsAsync<IdentityRequestDependencyException>(() =>
                 authenticationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIdentityRequestDependencyException))),
                      Times.Once);

            this.userManagerBrokerMock.Verify(broker =>
                broker.SelectUserByEmailAsync(It.IsAny<string>()),
                  Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagerBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAuthenticateIfExceptionsOccuresAndLogItAsnc()
        {
            //given
            IdentityRequest someRequest = CreateRandomIdentityRequest();
            Exception exception = new Exception();

            var failIdentityResponseServiceException =
                new FailIdentityRequestServiceException(exception);

            var expectedIdentityRequestServiceException =
                new IdentityRequestServiceException(failIdentityResponseServiceException);

            this.userManagerBrokerMock.Setup(broker =>
                broker.SelectUserByEmailAsync(someRequest.Email))
                   .ThrowsAsync(exception);

            //when
            ValueTask<IdentityResponse> authenticationTask =
                this.identityService.AuthenticateUserAsync(someRequest);

            //then
            await Assert.ThrowsAsync<IdentityRequestServiceException>(() =>
                 authenticationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIdentityRequestServiceException))),
                      Times.Once);

            this.userManagerBrokerMock.Verify(broker =>
                broker.SelectUserByEmailAsync(It.IsAny<string>()),
                  Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagerBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAuthenticateIfIdentityTokenEncryptionFailAndLogItAsnc()
        {
            IdentityRequest someRequest =
                CreateRandomIdentityRequest();

            var securityTokenEncryptionFailedException =
                new SecurityTokenEncryptionFailedException();

            var failIdentityRequestServiceException =
                new FailIdentityRequestServiceException(securityTokenEncryptionFailedException);

            var expectedIdentityRequestServiceException =
               new IdentityRequestServiceException(failIdentityRequestServiceException);

            this.userManagerBrokerMock.Setup(broker =>
                broker.SelectUserByEmailAsync(someRequest.Email))
                   .ThrowsAsync(securityTokenEncryptionFailedException);

            //when
            ValueTask<IdentityResponse> authenticationTask =
                this.identityService.AuthenticateUserAsync(someRequest);

            //then
            await Assert.ThrowsAsync<IdentityRequestServiceException>(() =>
                 authenticationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedIdentityRequestServiceException))),
                      Times.Once);

            this.userManagerBrokerMock.Verify(broker =>
                broker.SelectUserByEmailAsync(It.IsAny<string>()),
                  Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagerBrokerMock.VerifyNoOtherCalls();
        }
    }
}
