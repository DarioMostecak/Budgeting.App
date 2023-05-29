// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Accounts;
using Budgeting.App.Api.Models.Accounts.Exceptions;
using Moq;
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
    }
}
