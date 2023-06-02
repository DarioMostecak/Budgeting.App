// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Budgeting.App.Api.Brokers.DbTransactions
{
    public class DbTransactionBroker : IDbTransactionBroker
    {
        private readonly MongoClient mongoClient;
        private MongoDbOptions mongoDbOptions;
        private IClientSessionHandle clientSessionHandle;

        public DbTransactionBroker(IOptions<MongoDbOptions> mongoDbOptions)
        {
            this.mongoDbOptions = mongoDbOptions.Value;
            this.mongoClient = GetMongoClient(this.mongoDbOptions);
            this.clientSessionHandle = this.mongoClient.StartSession();
        }

        public void BeginTransaction() =>
           this.clientSessionHandle.StartTransaction();

        public void CommitTransaction() =>
            this.clientSessionHandle.CommitTransaction();

        public void RollBackTransaction() =>
            this.clientSessionHandle.AbortTransaction();

        public void DisposeTransaction() =>
            this.clientSessionHandle.Dispose();

        private MongoClient GetMongoClient(MongoDbOptions mongoDbOptions)
        {
            var connectionString = mongoDbOptions.ConnectionString;
            var client = new MongoClient(connectionString);

            return client;
        }
    }
}
