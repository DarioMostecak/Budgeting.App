// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Accounts;

namespace Budgeting.App.Api.Services.Foundations.Accounts
{
    public interface IAccountService
    {
        ValueTask<Account> AddAccountAsync(Account account);
        ValueTask<Account> RetrieveAccountByUserIdentityIdAsync(string userIdentityId);
        ValueTask<Account> RetrieveAccountByIdAsync(Guid accountId);
    }
}
