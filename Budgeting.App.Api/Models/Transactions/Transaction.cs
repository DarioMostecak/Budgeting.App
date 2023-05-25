// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Budgeting.App.Api.Models.Transactions
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public Guid AccountId { get; set; }
        public Guid CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }

        [BsonElement("time_created")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime TimeCreated { get; set; }

        [BsonElement("time_modify")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime TimeModify { get; set; }
    }
}
