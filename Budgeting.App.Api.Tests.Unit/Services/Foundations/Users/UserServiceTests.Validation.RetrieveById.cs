using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveIfIdIsEmptyAndLogItAsync()
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
            ValueTask<User> retrieveUserTask =
                this.userService.RetrieveUserByIdAsync(invalidId);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                retrieveUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                      Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagerBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveIfNotFoundAndLogItAsync()
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
            ValueTask<User> retrieveUserTask =
                this.userService.RetrieveUserByIdAsync(randomGuid);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                retrieveUserTask.AsTask());

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
    }
}
