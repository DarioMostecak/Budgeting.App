using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;
using MongoDB.Driver;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddWhenUserAlredyExistAndLogItAsync()
        {
            //given 
            User someUser = CreateUser();
            string somePassword = GetRandomPassword();

            MongoWriteException mongoWriteException =
                GetMongoWriteException();

            var alreadyExistUserException =
                new AlreadyExistsUserException(mongoWriteException);

            var expectedUserValidationException =
                new UserValidationException(alreadyExistUserException, alreadyExistUserException.Data);

            this.userManagerBrokerMock.Setup(manager =>
                manager.InsertUserAsync(It.IsAny<User>(), It.IsAny<string>()))
                        .ThrowsAsync(mongoWriteException);

            //when
            ValueTask<User> addApplicationUserTask =
                this.userService.AddUserAsync(someUser, somePassword);

            //then
            await Assert.ThrowsAsync<UserValidationException>(() =>
                addApplicationUserTask.AsTask());


            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedUserValidationException))),
                      Times.Once);

            this.userManagerBrokerMock.Verify(manager =>
               manager.SelectUserByEmailAsync(It.IsAny<string>()),
                  Times.Once);

            this.userManagerBrokerMock.Verify(manager =>
               manager.InsertUserAsync(It.IsAny<User>(), It.IsAny<string>()),
                  Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagerBrokerMock.VerifyNoOtherCalls();
        }
    }
}
