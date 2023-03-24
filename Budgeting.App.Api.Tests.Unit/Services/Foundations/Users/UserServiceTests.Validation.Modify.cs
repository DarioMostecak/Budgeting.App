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
        public async Task ShouldThrowValidationExceptionOnModifyIfApplicationUserIsNullAndLogItAsync()
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
        public async Task ShouldThrowValidationExceptionOnModifyIfApplicationUserIsInvalidAndLogItAsync(
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
    }
}
