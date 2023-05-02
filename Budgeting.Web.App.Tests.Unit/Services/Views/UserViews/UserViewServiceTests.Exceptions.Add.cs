using Budgeting.Web.App.Models.Users;
using Budgeting.Web.App.Models.Users.Exceptions;
using Budgeting.Web.App.Models.UserViews;
using Budgeting.Web.App.Models.UserViews.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Views.UserViews
{
    public partial class UserViewServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfUserValidationExceptionOccuresAndLogItAsync()
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

        [Fact]
        public async Task ShouldThrowUnauthorizedExceptionOnAddIfUserUnauthorizedEceptionOccuresAndLogItAsync()
        {
            //given
            UserView someUserView = CreateUserView();

            var exception =
                new Exception("Unauthorized exception");

            var failedUserUnauthorizedException =
                new FailedUserUnauthorizedException(exception);

            var userUnauthorizedException =
                new UserUnauthorizedException(failedUserUnauthorizedException);

            var expectedUserViewUnauthorizedException =
                new UserViewUnauthorizedException(userUnauthorizedException);

            this.userServiceMock.Setup(service =>
                service.AddUserAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                         .ThrowsAsync(userUnauthorizedException);

            //when
            ValueTask<UserView> addUserViewTask =
                this.userViewService.AddUserViewAsync(someUserView);

            //then
            await Assert.ThrowsAsync<UserViewUnauthorizedException>(() =>
                 addUserViewTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedUserViewUnauthorizedException))),
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

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfUserDependencyExceptionOccuresAndLogItAsync()
        {
            //given
            UserView someUserView = CreateUserView();

            var exception =
                new Exception("Failed user dpendency exception.");

            var failedUserDependencyException =
                new FailedUserDependencyException(exception);

            var userDependencyException =
                new UserDependencyException(failedUserDependencyException);

            var failedUserViewDependencyException =
                new FailedUserViewDependencyException(userDependencyException);

            var expectedUserViewDependencyException =
                new UserViewDependencyException(failedUserViewDependencyException);

            this.userServiceMock.Setup(service =>
                service.AddUserAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                       .ThrowsAsync(userDependencyException);

            //when
            ValueTask<UserView> addUserViewTask =
                this.userViewService.AddUserViewAsync(someUserView);

            //then
            await Assert.ThrowsAsync<UserViewDependencyException>(() =>
                addUserViewTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedUserViewDependencyException))),
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

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfUserServiceExceptionOccuresAndLogItAsync()
        {
            //given
            UserView someUserView = CreateUserView();

            var exception =
                new Exception("Failed user service error.");

            var failedUserServiceException =
                new FailedUserServiceException(exception);

            var userServiceExeption =
                new UserServiceException(failedUserServiceException);

            var failedUserViewException =
                new FailedUserViewServiceException(userServiceExeption);

            var expectedUserViewServiceException =
                new UserViewServiceException(failedUserViewException);

            this.userServiceMock.Setup(service =>
                service.AddUserAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                        .ThrowsAsync(userServiceExeption);

            //when
            ValueTask<UserView> addUserViewTask =
                this.userViewService.AddUserViewAsync(someUserView);

            //then
            await Assert.ThrowsAsync<UserViewServiceException>(() =>
                addUserViewTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedUserViewServiceException))),
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

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfExceptionOccuresAndLogItAsync()
        {
            //given
            UserView someUserView = CreateUserView();

            var exception =
                new Exception("System service error.");

            var failedUserViewException =
                new FailedUserViewServiceException(exception);

            var expectedUserViewServiceException =
                new UserViewServiceException(failedUserViewException);

            this.userServiceMock.Setup(service =>
                service.AddUserAsync(
                    It.IsAny<User>(),
                    It.IsAny<string>()))
                        .ThrowsAsync(exception);

            //when
            ValueTask<UserView> addUserViewTask =
                this.userViewService.AddUserViewAsync(someUserView);

            //then
            await Assert.ThrowsAsync<UserViewServiceException>(() =>
                addUserViewTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedUserViewServiceException))),
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
