// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Accounts;
using Budgeting.App.Api.Models.Accounts.Exceptions;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Accounts
{
    public partial class AccountServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfMongoExceptionOccurresAndLogItAsync()
        {
            //given
            Account someAccount = CreateRandomAccount();
            MongoException mongoException = GetMongoException();

            var failedAccountDependencyException =
                new FailedAccountDependencyException(mongoException);

            var expectedAccountDependencyException =
                new AccountDependencyException(failedAccountDependencyException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAccountAsync(It.IsAny<Account>()))
                       .ThrowsAsync(mongoException);

            //when
            ValueTask<Account> addAccountTask =
                this.accountService.AddAccountAsync(someAccount);

            //then
            await Assert.ThrowsAsync<AccountDependencyException>(() =>
                addAccountTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccountDependencyException))),
                     Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAccountAsync(It.IsAny<Account>()),
                 Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfExceptionOccurresAndLogItAsync()
        {
            //given
            Account someAccount = CreateRandomAccount();
            var exception = new Exception();

            var failedAccountServiceException =
                new FailedAccountServiceException(exception);

            var expectedAccountServiceException =
                new AccountServiceException(failedAccountServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAccountAsync(It.IsAny<Account>()))
                       .ThrowsAsync(exception);

            //when
            ValueTask<Account> addAccountTask =
                this.accountService.AddAccountAsync(someAccount);

            //then
            await Assert.ThrowsAsync<AccountServiceException>(() =>
                addAccountTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccountServiceException))),
                     Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAccountAsync(It.IsAny<Account>()),
                 Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
