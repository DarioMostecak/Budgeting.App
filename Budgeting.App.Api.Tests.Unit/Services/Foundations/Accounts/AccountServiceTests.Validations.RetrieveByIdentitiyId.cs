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

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByUserIdentityIdIfNotFoundAccountExceptionOccurresAndLogItAsync()
        {
            //given
            string someUserIdentityId = GetRandomIdentityUserId();
            Account invalidAccount = null;

            var notFoundAccountException =
                new NotFoundAccountException(someUserIdentityId);

            var expectedAccountValidationException =
                new AccountValidationException(
                    notFoundAccountException,
                    notFoundAccountException.Data);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByUserIdentityIdAsync(It.IsAny<string>()))
                       .ReturnsAsync(invalidAccount);

            //when
            ValueTask<Account> retrieveAccountByUserIdentityIdTask =
                this.accountService.RetrieveAccountByUserIdentityIdAsync(someUserIdentityId);

            //then
            await Assert.ThrowsAsync<AccountValidationException>(() =>
                retrieveAccountByUserIdentityIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAccountValidationException))),
                     Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByUserIdentityIdAsync(It.IsAny<string>()),
                 Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
