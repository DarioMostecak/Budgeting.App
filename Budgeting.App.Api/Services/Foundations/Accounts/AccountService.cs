// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Brokers.Loggings;
using Budgeting.App.Api.Models.Accounts;

namespace Budgeting.App.Api.Services.Foundations.Accounts
{
    public partial class AccountService : IAccountService
    {
        private readonly ILoggingBroker loggingBroker;

        public AccountService(ILoggingBroker loggingBroker)
        {
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Account> AddAccountAsync(Account account)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Account> RetrieveAccountById(Guid accountId)
        {
            throw new NotImplementedException();
        }
    }
}
