// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Attributes;
using Budgeting.App.Api.Models.Categories;

namespace Budgeting.App.Api.Models.Transactions
{
    [BsonCollection("transactions")]
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public Category Category { get; set; }
        public Type Type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModify { get; set; }
    }
}
