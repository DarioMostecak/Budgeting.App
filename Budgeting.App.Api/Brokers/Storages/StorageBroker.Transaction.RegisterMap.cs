// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Transactions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Budgeting.App.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public static void RegisterTransactionMap()
        {
            BsonClassMap.RegisterClassMap<Transaction>(transactionMap =>
            {
                transactionMap.MapIdField(transaction => transaction.TransactionId)
                               .SetSerializer(new StringSerializer(BsonType.String))
                                .SetElementName("_transaction_id")
                                 .SetIsRequired(true);

                transactionMap.MapIdField(transaction => transaction.Category)
                                .SetElementName("category")
                                 .SetIsRequired(true);

                transactionMap.MapIdField(transaction => transaction.Type)
                               .SetSerializer(new StringSerializer(BsonType.String))
                                .SetElementName("type")
                                 .SetIsRequired(true);

                transactionMap.MapIdField(transaction => transaction.Amount)
                               .SetSerializer(new StringSerializer(BsonType.Decimal128))
                                .SetElementName("amount")
                                 .SetIsRequired(true);

                transactionMap.MapIdField(transaction => transaction.Description)
                               .SetSerializer(new StringSerializer(BsonType.String))
                                .SetElementName("description");

                transactionMap.MapIdField(transaction => transaction.Note)
                               .SetSerializer(new StringSerializer(BsonType.String))
                                .SetElementName("note");

                transactionMap.MapField(transaction => transaction.TimeCreated)
                           .SetSerializer(new StringSerializer(BsonType.DateTime))
                            .SetElementName("time_created")
                             .SetIsRequired(true);

                transactionMap.MapField(transaction => transaction.TimeModify)
                           .SetSerializer(new StringSerializer(BsonType.DateTime))
                            .SetElementName("time_modify")
                             .SetIsRequired(true);

            });
        }
    }
}
