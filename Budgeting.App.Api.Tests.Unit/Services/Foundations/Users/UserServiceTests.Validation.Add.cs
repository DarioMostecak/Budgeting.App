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

        [Theory]
        [MemberData(nameof(InvalidDataUser))]
        public async Task ShouldThrowValidationExceptionOnAddIfUserAndPasswordIsInvalidAndLogItAsync(
            Guid invalidId,
            string invalidFirstName,
            string invalidLastName,
            string invalidEmail,
            DateTime invalidCreateDate,
            DateTime invalidUpdateDate,
            string invalidPassword)
        {
            //given 
            var password = invalidPassword;

            var invalidUser = new User
            {
                Id = invalidId,
                FirstName = invalidFirstName,
                LastName = invalidLastName,
                Email = invalidEmail,
                CreatedDate = invalidCreateDate,
                UpdatedDate = invalidUpdateDate
            };

            var invalidUserException = new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(User.Id),
                values: "Id isn't valid.");

            invalidUserException.AddData(
                key: nameof(User.FirstName),
                values: "Must be between 3 and 20 charachters long and can't be null or white space.");

            invalidUserException.AddData(
                key: nameof(User.LastName),
                values: "Must be between 3 and 20 charachters long and can't be null or white space.");

            invalidUserException.AddData(
                key: nameof(User.Email),
                values: "Email can't be white space or null and must be type of email format.");

            invalidUserException.AddData(
                key: nameof(User.CreatedDate),
                values: "Date is required.");

            invalidUserException.AddData(
                key: nameof(User.UpdatedDate),
                values: "Date is required.");

            invalidUserException.AddData(
                key: nameof(password),
                values: "Password must be between 8 and 25 charachters long, can't be white space or contain white space.");

            var expectedUserValidationException =
                new UserValidationException(
                    invalidUserException,
                    invalidUserException.Data);

            //when
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(invalidUser, invalidPassword);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                addUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedUserValidationException))),
                      Times.Once);

            this.userManagerBrokerMock.Verify(manager =>
                manager.SelectUserByEmailAsync(It.IsAny<string>()),
                  Times.Never);

            this.userManagerBrokerMock.Verify(manager =>
                manager.InsertUserAsync(It.IsAny<User>(), It.IsAny<string>()),
                  Times.Never);


            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagerBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfUserEmailAlredyExistAndLogItAsync()
        {
            //given
            User someApplicationUser = CreateUser();
            string somePassword = GetRandomPassword();

            var alredyExistUserException =
                new AlreadyExistsUserException();

            var expectedUserValidationException =
                new UserValidationException(
                    alredyExistUserException,
                    alredyExistUserException.Data);

            this.userManagerBrokerMock.Setup(manager =>
                manager.SelectUserByEmailAsync(It.IsAny<string>()))
                  .ReturnsAsync(someApplicationUser);

            //when
            ValueTask<User> addUserTask =
                this.userService.AddUserAsync(someApplicationUser, somePassword);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                addUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserValidationException))),
                      Times.Once());

            this.userManagerBrokerMock.Verify(manager =>
                manager.SelectUserByEmailAsync(It.IsAny<string>()),
                  Times.Once());

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagerBrokerMock.VerifyNoOtherCalls();
        }
    }
}
