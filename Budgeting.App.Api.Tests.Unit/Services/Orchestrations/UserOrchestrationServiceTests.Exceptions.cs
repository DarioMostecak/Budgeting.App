// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Orchestrations
{
    public partial class UserOrchestrationServiceTests
    {
        [Theory]
        [MemberData(nameof(UserOrchestrationDependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnRegisterIfDependencyOrServiceExceptionOccurresAndLogItAsync(
            Exception dependencyServiceException)
        {
            //given
            User randomUser = CreateUser();
            string randomPassword = GetRandomPassword();

            var expectedUserOrchestrationDependencyException =
                new UserOrchestrationDependencyException(dependencyServiceException);

            this.userServiceMock.Setup(broker =>
                broker.AddUserAsync(It.IsAny<User>(), It.IsAny<string>()))
                       .ThrowsAsync(dependencyServiceException);

            //when
            ValueTask<User> registerUserTask =
                this.userOrchestrationService.RegisterUserAsync(randomUser, randomPassword);

            //then
            await Assert.ThrowsAsync<UserOrchestrationDependencyException>(() =>
                registerUserTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(
                    SameExceptionAs(expectedUserOrchestrationDependencyException))),
                     Times.Once);

            this.userServiceMock.Verify(broker =>
                broker.AddUserAsync(It.IsAny<User>(), It.IsAny<string>()),
                 Times.Once);

            this.dbTransactionBrokerMock.Verify(broker =>
                broker.BeginTransaction(),
                 Times.Once);

            this.dbTransactionBrokerMock.Verify(broker =>
                broker.RollBackTransaction(),
                 Times.Once);

            this.dbTransactionBrokerMock.Verify(broker =>
                broker.DisposeTransaction(),
                 Times.Once);

            this.userServiceMock.VerifyNoOtherCalls();
            this.accountserviceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dbTransactionBrokerMock.VerifyNoOtherCalls();
            this.uniqueIDGeneratorBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
