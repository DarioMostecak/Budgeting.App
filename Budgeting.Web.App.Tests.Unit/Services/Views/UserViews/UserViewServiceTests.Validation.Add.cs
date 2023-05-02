using Budgeting.Web.App.Models.UserViews;
using Budgeting.Web.App.Models.UserViews.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Views.UserViews
{
    public partial class UserViewServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfUserViewIsNullAndLogItAsync()
        {
            //given
            UserView nullUserView = null;

            var nullUserViewException =
                new NullUserViewException();

            var expectedUserViewValidationException =
                new UserViewValidationException(
                    innerException: nullUserViewException,
                    data: nullUserViewException.Data);

            //when
            ValueTask<UserView> addUserViewTask =
                this.userViewService.AddUserViewAsync(nullUserView);

            //then
            await Assert.ThrowsAsync<UserViewValidationException>(() =>
                addUserViewTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedUserViewValidationException))),
                        Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBroker.VerifyNoOtherCalls();
            this.uniqueIDGeneratorBroker.VerifyNoOtherCalls();
            this.navigationBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidDataUserView))]
        public async Task ShouldThrowValidationExceptionOnAddIfUserViewIsInvalidAndLogItAsync(
            string invalidFirstName,
            string invalidLastName,
            string invalidEmail,
            string invalidPassword,
            string invalidConfirmPassword)
        {
            //given
            var invalidUserView = new UserView
            {
                FirstName = invalidFirstName,
                LastName = invalidLastName,
                Email = invalidEmail,
                Password = invalidPassword,
                ConfirmPassword = invalidConfirmPassword
            };

            var invalidUserViewException =
                new InvalidUserViewException();

            invalidUserViewException.AddData(
                key: nameof(UserView.FirstName),
                values: "Must be between 3 and 20 charachters " +
                        "long and can't be null or white space.");

            invalidUserViewException.AddData(
                key: nameof(UserView.LastName),
                values: "Must be between 3 and 20 charachters " +
                        "long and can't be null or white space.");

            invalidUserViewException.AddData(
                key: nameof(UserView.Email),
                values: "Email can't be white space or null and" +
                         " must be type of email format.");

            invalidUserViewException.AddData(
                key: nameof(UserView.Password),
                values: "Password must be between 8 and 25 charachters long," +
                        " can't be white space or contain white space.");

            invalidUserViewException.AddData(
                key: nameof(UserView.ConfirmPassword),
                values: "Confirm password must have same value as password.");

            var expectedUserViewValidationException =
                new UserViewValidationException(
                    innerException: invalidUserViewException,
                    data: invalidUserViewException.Data);

            //when
            ValueTask<UserView> addUserViewTask =
                this.userViewService.AddUserViewAsync(invalidUserView);

            //then
            await Assert.ThrowsAsync<UserViewValidationException>(() =>
                addUserViewTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserViewValidationException))),
                      Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBroker.VerifyNoOtherCalls();
            this.uniqueIDGeneratorBroker.VerifyNoOtherCalls();
            this.navigationBrokerMock.VerifyNoOtherCalls();
        }
    }
}
