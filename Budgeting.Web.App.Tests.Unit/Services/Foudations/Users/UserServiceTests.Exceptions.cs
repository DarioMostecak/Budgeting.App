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
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
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
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }
    }
}
