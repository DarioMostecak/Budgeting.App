// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Accounts;
using Budgeting.App.Api.Models.Accounts.Exceptions;
using MongoDB.Driver;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Accounts
{
    public partial class AccountServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdentityIdIfMongoExceptionOccuresAndLogItAsync()
        {
            //given
            string randomUserIdentityId = GetRandomIdentityUserId();
            MongoException mongoException = GetMongoException();

            var failedAccountDependencyException =
                new FailedAccountDependencyException(mongoException);

            var expectedAccountDependencyException =
                new AccountDependencyException(failedAccountDependencyException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByUserIdentityIdAsync(It.IsAny<string>()))
                       .ThrowsAsync(mongoException);

            //when
            ValueTask<Account> retrieveAccountByIdentityIdTask =
                this.accountService.RetrieveAccountByUserIdentityIdAsync(randomUserIdentityId);

            //then
            await Assert.ThrowsAsync<AccountDependencyException>(() =>
                 retrieveAccountByIdentityIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedAccountDependencyException))),
                      Times.Once);

            this.storageBrokerMock.Verify(broker =>
                 broker.SelectAccountByUserIdentityIdAsync(It.IsAny<string>()),
                  Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
