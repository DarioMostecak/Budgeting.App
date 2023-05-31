// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Accounts;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Accounts
{
    public partial class AccountServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveAccountByUserIdentityId()
        {
            //given
            Account randomAccount = CreateRandomAccount();
            Account storageAccount = randomAccount;
            Account expectedAccount = storageAccount.DeepClone();
            string userIdentityId = randomAccount.UserIdentityId;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAccountByUserIdentityIdAsync(It.IsAny<string>())).ReturnsAsync(storageAccount);

            //when
            Account actualAccount =
                await this.accountService.RetrieveAccountByUserIdentityIdAsync(userIdentityId);

            //then
            actualAccount.Should().BeEquivalentTo(expectedAccount);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAccountByUserIdentityIdAsync(It.IsAny<string>()),
                 Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
