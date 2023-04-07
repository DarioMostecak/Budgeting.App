using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsEmptyAndLogItAsync()
        {
            //given
            Guid invalidId = Guid.Empty;

            var invalidUserException =
                new InvalidUserException(
                    nameof(User.Id),
                    invalidId);

            var expectedUserValidationException =
                new UserValidationException(
                    invalidUserException,
                    invalidUserException.Data);

            //when
            ValueTask<User> removeUserByIdTask =
                this.userService.RemoveUserByIdAsync(invalidId);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                removeUserByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagerBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfUserNotFoundAndLogItAsync()
        {
            //given
            Guid randomGuid = Guid.NewGuid();
            User noStorageUser = null;

            var notFoundUserException =
                new NotFoundUserException(randomGuid);

            var expectedUserValidationException =
                new UserValidationException(
                    notFoundUserException,
                    notFoundUserException.Data);

            this.userManagerBrokerMock.Setup(manager =>
                manager.SelectUserByIdAsync(It.IsAny<Guid>()))
                  .ReturnsAsync(noStorageUser);

            //when
            ValueTask<User> removeUserTask =
                this.userService.RemoveUserByIdAsync(randomGuid);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                removeUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                      Times.Once);

            this.userManagerBrokerMock.Verify(manager =>
                manager.SelectUserByIdAsync(It.IsAny<Guid>()),
                  Times.Once);

            this.userManagerBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdentityResultSuccessIsFalseAndLogItAsync()
        {
            //given
            User someUser = CreateUser();

            IdentityError identityError =
                new IdentityError
                {
                    Code = "InvalidUserName",
                    Description = "Fail to Execute."
                };

            var invalidUserException =
                new InvalidUserException();

            invalidUserException.AddData(
                key: "InvalidUserName",
                values: "Fail to Execute.");

            var expectedValidationException =
                new UserValidationException(
                    invalidUserException,
                    invalidUserException.Data);

            this.userManagerBrokerMock.Setup(manager =>
                manager.SelectUserByIdAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(someUser);

            this.userManagerBrokerMock.Setup(manager =>
                manager.DeleteUserAsync(It.IsAny<User>()))
                   .ReturnsAsync(IdentityResult.Failed(identityError));

            //when
            ValueTask<User> removeUserByIdTask =
                this.userService.RemoveUserByIdAsync(someUser.Id);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                removeUserByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedValidationException))),
                      Times.Once());

            this.userManagerBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                  Times.Once());

            this.userManagerBrokerMock.Verify(broker =>
                broker.DeleteUserAsync(It.IsAny<User>()),
                  Times.Once());

            this.userManagerBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
