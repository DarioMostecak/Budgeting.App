using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;
using Force.DeepCloner;
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
        public async Task ShouldThrowValidationExceptionOnModifyIfUserIsNullAndLogItAsync()
        {
            //given
            User nullUser = null;

            var nullUserException =
                new NullUserException();

            var expectedUserValidationException =
                new UserValidationException(
                    nullUserException,
                    nullUserException.Data);

            //when
            ValueTask<User> modifyUserTask =
                 this.userService.ModifyUserAsync(nullUser);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                 modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedUserValidationException))),
                       Times.Once);

            this.userManagerBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidDataUser))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUserIsInvalidAndLogItAsync(
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
                values: new string[] { "Date is required.", $"Date is the same as {nameof(User.UpdatedDate)}" });

            var expectedUserValidationException =
                new UserValidationException(
                    invalidUserException,
                    invalidUserException.Data);

            //when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(invalidUser);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedUserValidationException))),
                      Times.Once);


            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagerBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUserIsNullAndLogItAsync()
        {
            //given 
            User randomUser = CreateUser();
            User nonExsistentUser = randomUser;
            User noUser = null;

            var notFoundUserException =
                new NotFoundUserException(nonExsistentUser.Id);

            var expectedUserValidationException =
                new UserValidationException(
                    notFoundUserException,
                    notFoundUserException.Data);

            this.userManagerBrokerMock.Setup(manager =>
                 manager.SelectUserByIdAsync(nonExsistentUser.Id))
                   .ReturnsAsync(noUser);

            //when
            ValueTask<User> modifyUserTask =
                 this.userService.ModifyUserAsync(randomUser);
            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                 modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedUserValidationException))),
                       Times.Once);

            this.userManagerBrokerMock.Verify(broker =>
                 broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.userManagerBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfIdAndCratedDatesAreNotSameAndUpdateDateIsSameAndLogItAsync()
        {
            //given
            User randomUser = CreateUser();
            DateTime sameDate = randomUser.UpdatedDate;
            DateTime differentDate = GetRandomDateTime();
            Guid differentId = Guid.NewGuid();
            User invalidUser = randomUser;
            User storageUser = randomUser.DeepClone();
            storageUser.Id = differentId;
            storageUser.UpdatedDate = sameDate;
            storageUser.CreatedDate = differentDate;

            var invalidUserException = new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(User.Id),
                values: $"Id is not the same as {nameof(User.Id)}");

            invalidUserException.AddData(
                key: nameof(User.CreatedDate),
                values: $"Date is not the same as {nameof(User.CreatedDate)}");

            invalidUserException.AddData(
                key: nameof(User.UpdatedDate),
                values: $"Date is the same as {nameof(User.UpdatedDate)}");

            var expectedUserValidationException =
                new UserValidationException(
                    invalidUserException,
                    invalidUserException.Data);

            this.userManagerBrokerMock.Setup(broker =>
                broker.SelectUserByIdAsync(invalidUser.Id))
                 .ReturnsAsync(storageUser);

            //when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(invalidUser);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                 modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameValidationExceptionAs(
                     expectedUserValidationException))),
                        Times.Once);

            this.userManagerBrokerMock.Verify(manager =>
                 manager.SelectUserByIdAsync(It.IsAny<Guid>()),
                   Times.Once);

            this.userManagerBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationErrorOnModifyIfIdentityResultSucessIsFalseAndLogItAsync()
        {
            //given
            User randomUser = CreateUser();
            User inputUser = randomUser;
            User storageUser = inputUser.DeepClone();
            storageUser.UpdatedDate = GetRandomDateTime();

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

            var expectedUserValidationException =
                new UserValidationException(
                    invalidUserException,
                    invalidUserException.Data);

            this.userManagerBrokerMock.Setup(manager =>
                manager.SelectUserByIdAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(storageUser);

            this.userManagerBrokerMock.Setup(manager =>
                manager.UpdateUserAsync(It.IsAny<User>()))
                   .ReturnsAsync(IdentityResult.Failed(identityError));

            //when
            ValueTask<User> modifyUserTask =
                this.userService.ModifyUserAsync(inputUser);

            //then
            await Assert.ThrowsAnyAsync<UserValidationException>(() =>
                modifyUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedUserValidationException))),
                      Times.Once());

            this.userManagerBrokerMock.Verify(broker =>
                broker.SelectUserByIdAsync(It.IsAny<Guid>()),
                  Times.Once());

            this.userManagerBrokerMock.Verify(broker =>
                broker.UpdateUserAsync(It.IsAny<User>()),
                  Times.Once());

            this.userManagerBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
