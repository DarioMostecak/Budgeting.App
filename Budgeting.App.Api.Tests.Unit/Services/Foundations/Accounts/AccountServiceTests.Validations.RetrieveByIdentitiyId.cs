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
        public async Task ShouldThrowValidationExceptionOnRetrieveByUserIdentityIdIfUserIdentityIdIsNotValidAndLogItAsync()
        {
            //given
            string invalidUserIdentityId = null;

            var invalidAccountException =
                new InvalidAccountException(
                    parameterName: "userIdentityId",
                    parameterValue: invalidUserIdentityId);

            var expectedAccountValidationException =
                new AccountValidationException(
                    invalidAccountException,
                    invalidAccountException.Data);

            //when
            ValueTask<Account> retrieveAccountByUserIdentityIdTask =
                this.accountService.RetrieveAccountByUserIdentityIdAsync(invalidUserIdentityId);

            //then
            await Assert.ThrowsAsync<AccountValidationException>(() =>
                retrieveAccountByUserIdentityIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccountValidationException))),
                     Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
