// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Accounts;

namespace Budgeting.App.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Account> InsertAccountAsync(Account account);
        ValueTask<Account> SelectAccountByUserIdentityIdAsync(string userIdentityId);
        ValueTask<Account> SelectAccountByIdAsync(Guid accountId);
    }
}
