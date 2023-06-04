// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Accounts;
using Budgeting.App.Api.Models.Users;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Orchestrations
{
    public partial class UserOrchestrationServiceTests
    {
        [Fact]
        public async Task ShouldRegisterUserAsync()
        {
            //given
            User randomUser = CreateUser();
            string randomPassword = GetRandomPassword();
            Account randomAccount = CreateRandomAccount();
            User inputUser = randomUser;
            string inputPassword = randomPassword;
            User newUser = inputUser;
            User expectedUser = newUser.DeepClone();
            string userIdentityId = randomUser.Id.ToString();
            Account inputAccount = randomAccount;
            inputAccount.UserIdentityId = userIdentityId;
            Account expectedAccount = inputAccount.DeepClone();

            this.userServiceMock.Setup(service =>
                service.AddUserAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(newUser);

            this.accountServiceMock.Setup(service =>
                service.AddAccountAsync(It.IsAny<Account>()))
                 .ReturnsAsync(inputAccount);

            //when
            User actualUser =
                await this.userOrchestrationService.RegisterUserAsync(inputUser, inputPassword);

            //then
            actualUser.Should().BeEquivalentTo(expectedUser);

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
                broker.CommitTransaction(),
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
