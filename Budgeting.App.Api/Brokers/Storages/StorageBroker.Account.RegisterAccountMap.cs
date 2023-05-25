// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Accounts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Budgeting.App.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public static void RegisterAccountMap()
        {
            BsonClassMap.RegisterClassMap<Account>(accountMap =>
            {
                accountMap.MapIdField(account => account.AccountId)
                           .SetSerializer(new StringSerializer(BsonType.String))
                             .SetElementName("_account_id")
                              .SetIsRequired(true);

                accountMap.MapField(account => account.UserIdentityId)
                           .SetSerializer(new StringSerializer(BsonType.String))
                            .SetElementName("user_identity_id")
                             .SetIsRequired(true);

                accountMap.MapField(account => account.Balance)
                           .SetSerializer(new StringSerializer(BsonType.Decimal128))
                            .SetElementName("balance")
                             .SetIsRequired(true);

                accountMap.MapField(accountMap => accountMap.Transactions)
                           .SetElementName("transactions");

                accountMap.MapField(account => account.TimeCreated)
                           .SetSerializer(new StringSerializer(BsonType.DateTime))
                            .SetElementName("time_created")
                             .SetIsRequired(true);

                accountMap.MapField(account => account.TimeModify)
                           .SetSerializer(new StringSerializer(BsonType.DateTime))
                            .SetElementName("time_modify")
                             .SetIsRequired(true);

            });
        }
    }
}
