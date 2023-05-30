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
        public async Task ShouldAddAccountAsync()
        {
            //given
            Account randomAccount = CreateRandomAccount();
            Account inputAccount = randomAccount;
            Account storageAccount = inputAccount;
            Account expectedAccount = inputAccount.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAccountAsync(It.IsAny<Account>()))
                 .ReturnsAsync(storageAccount);

            //when
            Account actualAccount =
                await this.accountService.AddAccountAsync(inputAccount);

            //then
            actualAccount.Should().BeEquivalentTo(expectedAccount);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAccountAsync(It.IsAny<Account>()),
                 Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
