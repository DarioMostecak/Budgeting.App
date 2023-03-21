using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddWhenUserIsNullAndLogItAsync()
        {
            //given
            User invalidUser = null;
            string somePassword = GetRandomPassword();

            var nullUserException =
                new NullUserException();

            var expectedUserValidationException =
                new UserValidationException(
                    nullUserException,
                    nullUserException.Data);

            //when
            ValueTask<User> addApplicationUserTask =
                this.userService.AddUserAsync(invalidUser, somePassword);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                addApplicationUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                      Times.Once());

            this.userManagerBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddWhenPasswordIsNullAndLogItAsync()
        {
            //given
            User someUser = CreateUser();
            string invalidPassword = null;

            var nullUserPasswordException =
                new NullUserPasswordException();

            var expectedUserValidationException =
                new UserValidationException(
                    nullUserPasswordException,
                    nullUserPasswordException.Data);

            //when
            ValueTask<User> addApplicationUserTask =
                this.userService.AddUserAsync(someUser, invalidPassword);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                addApplicationUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                      Times.Once());

            this.userManagerBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}
