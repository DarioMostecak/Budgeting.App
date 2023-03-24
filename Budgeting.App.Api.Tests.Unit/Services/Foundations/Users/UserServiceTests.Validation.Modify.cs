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
    }
}
