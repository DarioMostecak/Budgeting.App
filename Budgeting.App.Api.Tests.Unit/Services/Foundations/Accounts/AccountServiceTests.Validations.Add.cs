// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Accounts;
using Budgeting.App.Api.Models.Accounts.Exceptions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Accounts
{
    public partial class AccountServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfNullAccoutExceptionOccurresAndLogItAsync()
        {
            //given
            Account nullAccount = null;

            var nullAccountException =
                new NullAccountException();

            var expectedAccountValidationException =
                new AccountValidationException(
                    nullAccountException,
                    nullAccountException.Data);

            //when
            ValueTask<Account> addAccountTask =
                this.accountService.AddAccountAsync(nullAccount);

            //then
            await Assert.ThrowsAsync<AccountValidationException>(() =>
                addAccountTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccountValidationException))),
                     Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidAccountData))]
        public async Task ShouldThrowValidationExceptionOnAddIfAccountIsInvalidAndLogItAsync(
            Guid invalidId,
            string invalidUserIdentityId,
            decimal invalidBalance,
            DateTime invalidDate)
        {
            //given
            var invalidAccount = new Account
            {
                AccountId = invalidId,
                UserIdentityId = invalidUserIdentityId,
                Balance = invalidBalance,
                TimeCreated = invalidDate,
                TimeModify = invalidDate
            };

            var invalidAccountException = new InvalidAccountException();

            invalidAccountException.AddData(
                key: nameof(Account.AccountId),
                values: "Id isn't valid.");

            invalidAccountException.AddData(
                key: nameof(Account.UserIdentityId),
                values: "UserIdentityId isn't valid.");

            invalidAccountException.AddData(
                key: nameof(Account.Balance),
                values: "Balance must be zero.");

            invalidAccountException.AddData(
                key: nameof(Account.TimeCreated),
                values: "Date is required.");

            invalidAccountException.AddData(
                key: nameof(Account.TimeModify),
                values: "Date is required.");

            var expectedAccountValidationException =
                new AccountValidationException(
                    invalidAccountException,
                    invalidAccountException.Data);

            //when
            ValueTask<Account> addAccountTask =
                this.accountService.AddAccountAsync(invalidAccount);

            //then
            await Assert.ThrowsAsync<AccountValidationException>(() =>
                addAccountTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccountValidationException))),
                     Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
