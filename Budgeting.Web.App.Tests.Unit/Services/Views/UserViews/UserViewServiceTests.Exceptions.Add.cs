using Budgeting.Web.App.Models.Users;
using Budgeting.Web.App.Models.Users.Exceptions;
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
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfErrorOccuresAndLogItAsync()
        {
            //given
            UserView someUserView = CreateUserView();

            var invalidUserException =
               new InvalidUserException();

            invalidUserException.AddData(
                key: nameof(User.Id),
                values: "Id is required.");

            var userDependencyValidationException =
                new UserDependencyValidationException(invalidUserException);

            var expectedUserViewDependencyValidationException =
                new UserViewDependencyValidationException(userDependencyValidationException);

            this.userServiceMock.Setup(service =>
                service.AddUserAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                        .ThrowsAsync(userDependencyValidationException);

            //when
            ValueTask<UserView> addUserViewTask =
                 this.userViewService.AddUserViewAsync(someUserView);

            //then
            await Assert.ThrowsAsync<UserViewDependencyValidationException>(() =>
                 addUserViewTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                    expectedUserViewDependencyValidationException))),
                       Times.Once);

            this.userServiceMock.Verify(service =>
                 service.AddUserAsync(
                      It.IsAny<User>(),
                      It.IsAny<string>()),
                        Times.Once);

            this.dateTimeBroker.Verify(broker =>
                 broker.GetCurrentDateTime(),
                    Times.Once);

            this.uniqueIDGeneratorBroker.Verify(broker =>
                 broker.GenerateUniqueID(),
                    Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBroker.VerifyNoOtherCalls();
            this.uniqueIDGeneratorBroker.VerifyNoOtherCalls();
            this.navigationBrokerMock.VerifyNoOtherCalls();
        }
    }
}
