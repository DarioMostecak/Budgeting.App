using Budgeting.Web.App.Models.ExceptionModels;
using Budgeting.Web.App.Models.Users;
using Budgeting.Web.App.Models.Users.Exceptions;
using Moq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Users
{
    public partial class UserServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyApiExceptions))]
        public async Task ShouldThrowDependencyExceptionOnAddUserIfDependencyErrorOcccurAndLogItAsync(
            Exception httpResponseDependencyException)
        {
            //given
            User someUser = CreateUser();
            var somePassword = CreatePassword();

            var failUserDependencyValidationException =
                new FailedUserDependencyException(httpResponseDependencyException);

            var expectedUserDependencyException =
                new UserDependencyException(failUserDependencyValidationException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostUserAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                        .ThrowsAsync(httpResponseDependencyException);

            //when
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(someUser, somePassword);

            //then
            await Assert.ThrowsAsync<UserDependencyException>(() =>
                 addUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(expectedUserDependencyException))),
                    Times.Once);

            this.apiBrokerMock.Verify(broker =>
                broker.PostUserAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()),
                       Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowUnauthorizeExceptionOnAddUserIfErrorOccuresAndLogItAsync()
        {
            //given
            User someUser = CreateUser();
            string password = CreatePassword();
            string exceptionMessage = "Request unauthorize.";
            var responseMessage = new HttpResponseMessage();

            var httpResponseUnauthorizedException =
                new HttpResponseUnauthorizedException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            var failedUserUnauthorizedException =
                new FailedUserUnauthorizedException(httpResponseUnauthorizedException);

            var expectedUserUnauthorizedExcepton =
                new UserUnauthorizedException(failedUserUnauthorizedException);

            this.apiBrokerMock.Setup(broker =>
                 broker.PostUserAsync(
                     It.IsAny<User>(),
                     It.IsAny<string>()))
                          .ThrowsAsync(httpResponseUnauthorizedException);

            //when
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(someUser, password);

            //then
            await Assert.ThrowsAsync<UserUnauthorizedException>(() =>
                 addUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedUserUnauthorizedExcepton))),
                       Times.Once);

            this.apiBrokerMock.Verify(broker =>
               broker.PostUserAsync(
                   It.IsAny<User>(),
                   It.IsAny<string>()),
                      Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddUserIfErrorOccuresAndLogItAsync()
        {
            //given
            User someUser = CreateUser();
            string password = CreatePassword();
            var exception = new Exception();

            var failedUserServiceException =
                new FailedUserServiceException(exception);

            var expectedUserServiceException =
                new UserServiceException(failedUserServiceException);

            this.apiBrokerMock.Setup(broker =>
                 broker.PostUserAsync(
                     It.IsAny<User>(),
                     It.IsAny<string>()))
                         .ThrowsAsync(exception);

            //when
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(someUser, password);

            //then
            await Assert.ThrowsAsync<UserServiceException>(() =>
                 addUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedUserServiceException))),
                   Times.Once);

            this.apiBrokerMock.Verify(broker =>
                broker.PostUserAsync(It.IsAny<User>(), It.IsAny<string>()),
                   Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }

        [Fact]
        public async Task ShoudThrowDependencyValidationExceptionOnAddIfHttpResponseNotFoundErrorOccuresAndLogItAsync()
        {
            //given
            User someUser = CreateUser();
            string password = CreatePassword();
            string exceptionMessage = "Request is invalid.";
            var responseMessage = new HttpResponseMessage();

            var httpResponseNotFoundException =
                new HttpResponseNotFoundException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            var failedUserDependencyException =
                new FailedUserDependencyException(httpResponseNotFoundException);

            var expectedDependencyValidationException =
                new UserDependencyValidationException(failedUserDependencyException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostUserAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                        .ThrowsAsync(httpResponseNotFoundException);

            //when 
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(someUser, password);

            //then
            await Assert.ThrowsAsync<UserDependencyValidationException>(() =>
                  addUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDependencyValidationException))),
                        Times.Once);

            this.apiBrokerMock.Verify(broker =>
                 broker.PostUserAsync(
                     It.IsAny<User>(),
                     It.IsAny<string>()),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfHttpBadRequestErrorOccuresAndLogItAsync()
        {
            //given
            User someUser = CreateUser();
            string password = CreatePassword();
            string exceptionMessage = "Request is invalid.";
            var responseMessage = new HttpResponseMessage();

            var httpResponseBadRequestException =
                new HttpResponseBadRequestException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            var invalidUserException =
                new InvalidUserException(httpResponseBadRequestException);

            var expectedDependencyValidationException =
                new UserDependencyValidationException(invalidUserException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostUserAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                        .ThrowsAsync(httpResponseBadRequestException);

            //when 
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(someUser, password);

            //then
            await Assert.ThrowsAsync<UserDependencyValidationException>(() =>
                  addUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDependencyValidationException))),
                        Times.Once);

            this.apiBrokerMock.Verify(broker =>
                 broker.PostUserAsync(
                     It.IsAny<User>(),
                     It.IsAny<string>()),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfHttpConflictErrorOccureAndLogItAsync()
        {
            //given
            User someUser = CreateUser();
            string password = CreatePassword();
            string exceptionMessage = "Request is invalid.";
            var responseMessage = new HttpResponseMessage();

            var httpResponseConflictException =
                new HttpResponseConflictException(
                    responseMessage: responseMessage,
                    message: exceptionMessage);

            var alreadyExistsUserException =
                new AlreadyExistsUserException(httpResponseConflictException);

            var expectedDependencyValidationException =
                new UserDependencyValidationException(alreadyExistsUserException);

            this.apiBrokerMock.Setup(broker =>
                broker.PostUserAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                        .ThrowsAsync(httpResponseConflictException);

            //when 
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(someUser, password);

            //then
            await Assert.ThrowsAsync<UserDependencyValidationException>(() =>
                  addUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedDependencyValidationException))),
                        Times.Once);

            this.apiBrokerMock.Verify(broker =>
                 broker.PostUserAsync(
                     It.IsAny<User>(),
                     It.IsAny<string>()),
                        Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
