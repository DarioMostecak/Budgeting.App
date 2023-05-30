// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Brokers.Loggings;
using Budgeting.App.Api.Brokers.Storages;
using Budgeting.App.Api.Models.Accounts;
using Budgeting.App.Api.Models.ExceptionModels;
using Budgeting.App.Api.Services.Foundations.Accounts;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Tynamix.ObjectFiller;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Accounts
{
    public partial class AccountServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IAccountService accountService;

        public AccountServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.accountService = new AccountService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static Account CreateRandomAccount() =>
            CreateRandomAccountFiller(dates: DateTime.UtcNow).Create();

        private static MongoException GetMongoException() =>
            (MongoException)FormatterServices.GetSafeUninitializedObject(typeof(MongoException));

        private static MongoWriteException GetMongoWriteException() =>
            (MongoWriteException)FormatterServices.GetSafeUninitializedObject(typeof(MongoWriteException));

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
            actualException.Message == expectedException.Message
            && (actualException.InnerException as ExceptionModel).DataEquals(expectedException.InnerException.Data);

        }

        private static Filler<Account> CreateRandomAccountFiller(DateTime dates)
        {
            var filler = new Filler<Account>();
            Guid accountId = Guid.NewGuid();
            string userIdentityId = Guid.NewGuid().ToString();

            filler.Setup()
                .OnProperty(account => account.AccountId).Use(accountId)
                .OnProperty(account => account.UserIdentityId).Use(userIdentityId)
                .OnProperty(account => account.Balance).Use(0)
                .OnProperty(account => account.TimeCreated).Use(dates)
                .OnProperty(account => account.TimeModify).Use(dates.AddDays(10));

            return filler;
        }

        private static IEnumerable<object[]> InvalidAccountData() =>
            new List<object[]>
            {
                new object[] {Guid.Empty, null, 20M, DateTime.MinValue}
            };

    }
}
