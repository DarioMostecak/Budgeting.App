// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Accounts;
using MongoDB.Driver;

namespace Budgeting.App.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        private IMongoCollection<Account> accountCollection;

        public async ValueTask<Account> InsertAccountAsync(Account account)
        {
            this.accountCollection =
                this.db.GetCollection<Account>(GetCollectionName<Account>());

            await this.accountCollection.InsertOneAsync(account);

            return account;
        }

        public async ValueTask<Account> SelectAccountByUserIdentityIdAsync(string userIdentityId)
        {
            this.accountCollection =
                this.db.GetCollection<Account>(GetCollectionName<Account>());

            return await this.accountCollection
                .Find(account => account.UserIdentityId == userIdentityId)
                 .FirstOrDefaultAsync();
        }

        public async ValueTask<Account> SelectAccountByIdAsync(Guid accountId)
        {
            this.accountCollection =
                this.db.GetCollection<Account>(GetCollectionName<Account>());

            return await this.accountCollection
                .Find(account => account.AccountId == accountId)
                 .FirstOrDefaultAsync();
        }
    }
}
