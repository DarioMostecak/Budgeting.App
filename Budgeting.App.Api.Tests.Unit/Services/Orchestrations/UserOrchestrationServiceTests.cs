// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Brokers.DateTimes;
using Budgeting.App.Api.Brokers.DbTransactions;
using Budgeting.App.Api.Brokers.Loggings;
using Budgeting.App.Api.Brokers.UniqueIDGenerators;
using Budgeting.App.Api.Services.Foundations.Accounts;
using Budgeting.App.Api.Services.Foundations.Users;
using Budgeting.App.Api.Services.Orchestrations.Users;
using Moq;

namespace Budgeting.App.Api.Tests.Unit.Services.Orchestrations
{
    public class UserOrchestrationServiceTests
    {
        private readonly Mock<IUserService> userServiceMock;
        private readonly Mock<IAccountService> accountserviceMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IDbTransactionBroker> dbTransactionBrokerMock;
        private readonly Mock<IUniqueIDGeneratorBroker> uniqueIDGeneratorBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly IUserOrchestrationService userOrchestrationService;

        public UserOrchestrationServiceTests()
        {
            this.userServiceMock = new Mock<IUserService>();
            this.accountserviceMock = new Mock<IAccountService>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.dbTransactionBrokerMock = new Mock<IDbTransactionBroker>();
            this.uniqueIDGeneratorBrokerMock = new Mock<IUniqueIDGeneratorBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();

            this.userOrchestrationService = new UserOrchestrationService(
                userService: this.userServiceMock.Object,
                accountService: this.accountserviceMock.Object,
                loggingBroker: this.loggingBrokerMock.Object,
                dbTransactionBroker: this.dbTransactionBrokerMock.Object,
                uniqueIDGeneratorBroker: this.uniqueIDGeneratorBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object);
        }
    }
}
