﻿using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveIfMongoExceptionErrorOccuresAndLogItAsync()
        {
            //given
            Guid someGuid = Guid.NewGuid();
            MongoException mongoException = GetMongoException();

            var failUserServiceException =
                new FailedUserServiceException(mongoException);

            var expectedUserDependencyException =
                new UserDependencyException(failUserServiceException);

            this.userManagerBrokerMock.Setup(manager =>
                manager.SelectUserByIdAsync(It.IsAny<Guid>()))
                  .ThrowsAsync(mongoException);

            //when
            ValueTask<User> retreiveByIdUserTask =
                this.userService.RetrieveUserById(someGuid);

            //then
            await Assert.ThrowsAsync<UserDependencyException>(() =>
                retreiveByIdUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserDependencyException))),
                      Times.Once);

            this.userManagerBrokerMock.Verify(manager =>
                manager.SelectUserByIdAsync(It.IsAny<Guid>()),
                  Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.userManagerBrokerMock.VerifyNoOtherCalls();
        }
    }
}