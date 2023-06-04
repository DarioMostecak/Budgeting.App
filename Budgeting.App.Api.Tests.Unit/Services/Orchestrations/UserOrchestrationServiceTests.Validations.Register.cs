// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Orchestrations
{
    public partial class UserOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRegisterIfUserIsNullAndLogItAsync()
        {
            //given
            User nullUser = null;
            string randomPassword = GetRandomPassword();
            var nullUserException = new NullUserException();

            var expectedUserOrchestrationValidationException =
                new UserOrchestrationValidationException(nullUserException);

            //when
            ValueTask<User> registerUserAsyncTask =
                this.userOrchestrationService.RegisterUserAsync(nullUser, randomPassword);

            //then
            await Assert.ThrowsAsync<UserOrchestrationValidationException>(() =>
                registerUserAsyncTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserOrchestrationValidationException))),
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

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRegisterIfUserPasswordIsNullAndLogItAsync()
        {
            //given
            User randomUser = CreateUser();
            string nullPassword = null;

            var nullUserPasswordException =
                new NullUserPasswordException();

            var expectedUserOrchestrationValidationException =
                new UserOrchestrationValidationException(nullUserPasswordException);

            //when
            ValueTask<User> registerUserAsyncTask =
                this.userOrchestrationService.RegisterUserAsync(randomUser, nullPassword);

            //then
            await Assert.ThrowsAsync<UserOrchestrationValidationException>(() =>
                registerUserAsyncTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserOrchestrationValidationException))),
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
