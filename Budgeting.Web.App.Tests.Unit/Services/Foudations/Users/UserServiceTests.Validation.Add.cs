using Budgeting.Web.App.Models.Users;
using Budgeting.Web.App.Models.Users.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfUserIsNullAndLogItAsync()
        {
            //given
            User nullUser = null;
            string password = CreatePassword();

            var nullUserException =
                new NullUserException();

            var expectedUserValidationException =
                new UserValidationException(
                     nullUserException,
                     nullUserException.Data);

            //when
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(nullUser, password);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                 addUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                      Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfUserPasswordIsNull()
        {
            User someUser = CreateUser();
            string password = null;

            var nullUserPasswordException =
                new NullUserPasswordException();

            var expectedUserValidationException =
                new UserValidationException(
                    nullUserPasswordException,
                    nullUserPasswordException.Data);

            //when
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(someUser, password);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                 addUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedUserValidationException))),
                       Times.Once);

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidUserData))]
        public async Task ShoudThrowValidationExceptionOnAddIfUserIsInvalidAndLogItAsync(
            Guid invalidUserId,
            string invalidFirstName,
            string invalidLastName,
            string invalidEmail,
            DateTime invalidCreatedDate,
            DateTime invalidUpdatedDate,
            string invalidPassword)
        {
            //given
            string password = invalidPassword;

            var invalidUser = new User
            {
                Id = invalidUserId,
                FirstName = invalidFirstName,
                LastName = invalidLastName,
                Email = invalidEmail,
                CreatedDate = invalidCreatedDate,
                UpdatedDate = invalidUpdatedDate,
            };

            var invalidUserException =
                new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(User.Id),
                values: "Id is required.");

            invalidUserException.AddData(
                key: nameof(User.FirstName),
                values: "Value can't be null, white space or empty.");

            invalidUserException.AddData(
                key: nameof(User.LastName),
                values: "Value can't be null, white space or empty.");

            invalidUserException.AddData(
                key: nameof(User.Email),
                values: "Value can't be null, white space or empty.");

            invalidUserException.AddData(
                key: nameof(User.CreatedDate),
                values: "Date is required.");

            invalidUserException.AddData(
                key: nameof(User.UpdatedDate),
                values: "Date is required.");

            invalidUserException.AddData(
                key: nameof(password),
                values: "Value can't be null, white space or empty.");

            var expectedUserValidationException =
                new UserValidationException(
                    innerException: invalidUserException,
                    data: invalidUserException.Data);

            //when
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(invalidUser, password);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                addUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                       Times.Once());

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
