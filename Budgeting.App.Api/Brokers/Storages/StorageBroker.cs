// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Attributes;
using MongoDB.Driver;

namespace Budgeting.App.Api.Brokers.Storages
{
    public partial class StorageBroker : IStorageBroker
    {
        private readonly IConfiguration configuration;
        private readonly IMongoDatabase db;

        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            RegisterClassMap();
            this.db = GetDatabase();
        }

        private string GetCollectionName<T>() where T : class
        {
            return (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                .FirstOrDefault() as BsonCollectionAttribute)!.CollectionName;
        }

        private IMongoDatabase GetDatabase()
        {
            var connectionString = this.configuration["BudgedDatabaseSettings:ConnectionString"];
            var client = new MongoClient(connectionString);

            return client
                .GetDatabase(this.configuration["BudgedDatabaseSettings:DatabaseName"]);
        }

        public static void RegisterClassMap()
        {
            RegisterAccountMap();
            RegisterTransactionMap();
        }
    }
}
