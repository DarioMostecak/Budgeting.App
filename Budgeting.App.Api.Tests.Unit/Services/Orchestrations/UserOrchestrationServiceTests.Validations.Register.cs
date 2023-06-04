// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Accounts;
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
            this.accountServiceMock.VerifyNoOtherCalls();
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
            this.accountServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dbTransactionBrokerMock.VerifyNoOtherCalls();
            this.uniqueIDGeneratorBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRegisterIfUserPasswordIsEmptyAndLogItAsync()
        {
            //given
            User randomUser = CreateUser();
            string nullPassword = string.Empty;

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
            this.accountServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dbTransactionBrokerMock.VerifyNoOtherCalls();
            this.uniqueIDGeneratorBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRegisterIfCreatedUserIsNullAndLogItAsync()
        {
            //given
            User randomUser = CreateUser();
            string randomPassword = GetRandomPassword();
            User nullUser = null;

            var failedOperationUserOrchestrationException =
                new FailedOperationUserOrchestrationException();

            var expectedUserOrchestrationDependencyException =
                new UserOrchestrationDependencyException(failedOperationUserOrchestrationException);

            this.userServiceMock.Setup(service =>
                service.AddUserAsync(It.IsAny<User>(), It.IsAny<string>()))
                        .ReturnsAsync(nullUser);

            //when
            ValueTask<User> registerUserAsyncTask =
                this.userOrchestrationService.RegisterUserAsync(randomUser, randomPassword);

            //then
            await Assert.ThrowsAsync<UserOrchestrationDependencyException>(() =>
                registerUserAsyncTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserOrchestrationDependencyException))),
                     Times.Once);

            this.userServiceMock.Verify(service =>
                service.AddUserAsync(It.IsAny<User>(), It.IsAny<string>()),
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
            this.accountServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dbTransactionBrokerMock.VerifyNoOtherCalls();
            this.uniqueIDGeneratorBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();


        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRegisterIfNewUserAccountIsNullandLogItAsync()
        {
            //given
            User randomUser = CreateUser();
            string randomPassword = GetRandomPassword();
            Account nullAccount = null;

            var failedOperationUserOrchestrationException =
                new FailedOperationUserOrchestrationException();

            var expectedUserOrchestrationDependencyException =
                new UserOrchestrationDependencyException(failedOperationUserOrchestrationException);

            this.userServiceMock.Setup(service =>
                service.AddUserAsync(It.IsAny<User>(), It.IsAny<string>()))
                       .ReturnsAsync(randomUser);

            this.accountServiceMock.Setup(service =>
                service.AddAccountAsync(It.IsAny<Account>()))
                       .ReturnsAsync(nullAccount);

            //when
            ValueTask<User> registerUserAsyncTask =
                this.userOrchestrationService.RegisterUserAsync(randomUser, randomPassword);

            //then
            await Assert.ThrowsAsync<UserOrchestrationDependencyException>(() =>
                registerUserAsyncTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserOrchestrationDependencyException))),
                     Times.Once);

            this.userServiceMock.Verify(service =>
                service.AddUserAsync(It.IsAny<User>(), It.IsAny<string>()),
                 Times.Once);

            this.uniqueIDGeneratorBrokerMock.Verify(broker =>
                broker.GenerateUniqueID(),
                 Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                 Times.Once);

            this.accountServiceMock.Verify(service =>
                service.AddAccountAsync(It.IsAny<Account>()),
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
            this.accountServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dbTransactionBrokerMock.VerifyNoOtherCalls();
            this.uniqueIDGeneratorBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRegisterIfAccuntIdentityIdDoNotEqualToUserIdAndLogItAsync()
        {
            //given
            User randomUser = CreateUser();
            string randomPassword = GetRandomPassword();
            Account randomAccount = CreateRandomAccount();

            var failedOperationUserOrchestrationException =
                new FailedOperationUserOrchestrationException();

            var expectedUserOrchestrationDependencyException =
                new UserOrchestrationDependencyException(failedOperationUserOrchestrationException);

            this.userServiceMock.Setup(service =>
                service.AddUserAsync(It.IsAny<User>(), It.IsAny<string>()))
                       .ReturnsAsync(randomUser);

            this.accountServiceMock.Setup(service =>
                service.AddAccountAsync(It.IsAny<Account>()))
                       .ReturnsAsync(randomAccount);

            //when
            ValueTask<User> registerUserAsyncTask =
                this.userOrchestrationService.RegisterUserAsync(randomUser, randomPassword);

            //then
            await Assert.ThrowsAsync<UserOrchestrationDependencyException>(() =>
                registerUserAsyncTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedUserOrchestrationDependencyException))),
                     Times.Once);

            this.userServiceMock.Verify(service =>
                service.AddUserAsync(It.IsAny<User>(), It.IsAny<string>()),
                 Times.Once);

            this.uniqueIDGeneratorBrokerMock.Verify(broker =>
                broker.GenerateUniqueID(),
                 Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                 Times.Once);

            this.accountServiceMock.Verify(service =>
                service.AddAccountAsync(It.IsAny<Account>()),
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
            this.accountServiceMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dbTransactionBrokerMock.VerifyNoOtherCalls();
            this.uniqueIDGeneratorBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
