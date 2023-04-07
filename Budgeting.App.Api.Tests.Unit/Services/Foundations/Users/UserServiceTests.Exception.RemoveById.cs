using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveIfMongoExceptionErrorOccuresAndLogItAsync()
        {
            //given
            Guid someGuid = Guid.NewGuid();
            MongoException mongoException = GetMongoException();

            var failUserServiceException =
                new FailedUserServiceException(mongoException);

            var expectedUserDependencyException =
                new UserDependencyException(failUserServiceException);

            this.userManagerBrokerMock.Setup(manager =>
                manager.SelectUserByIdAsync(It.IsAny<Guid>()))
                  .ThrowsAsync(mongoException);

            //when
            ValueTask<User> removeUserByIdTask =
                this.userService.RemoveUserByIdAsync(someGuid);

            //then
            await Assert.ThrowsAsync<UserDependencyException>(() =>
                removeUserByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserDependencyException))),
                      Times.Once);

            this.userManagerBrokerMock.Verify(manager =>
                manager.SelectUserByIdAsync(It.IsAny<Guid>()),
                  Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagerBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            //given
            Guid someGuid = Guid.NewGuid();
            Exception exception = new Exception();

            var failUserDependencyException =
                new FailedUserServiceException(exception);

            var expectedUserServiveException =
                new UserServiceException(failUserDependencyException);

            this.userManagerBrokerMock.Setup(manager =>
                manager.SelectUserByIdAsync(It.IsAny<Guid>()))
                  .ThrowsAsync(exception);

            //when
            ValueTask<User> removeUserByIdTask =
                this.userService.RemoveUserByIdAsync(someGuid);

            //then
            await Assert.ThrowsAsync<UserServiceException>(() =>
                removeUserByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserServiveException))),
                      Times.Once);

            this.userManagerBrokerMock.Verify(manager =>
                manager.SelectUserByIdAsync(It.IsAny<Guid>()),
                  Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagerBrokerMock.VerifyNoOtherCalls();
        }
    }
}
