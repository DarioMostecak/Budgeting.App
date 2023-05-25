// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Attributes;
using Budgeting.App.Api.Models.Transactions;

namespace Budgeting.App.Api.Models.Accounts
{
    [BsonCollection("account")]
    public class Account
    {
        public Guid AccountId { get; set; }
        public string UserIdentityId { get; set; }
        public decimal Balance { get; set; }
        public IList<Transaction> Transactions { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModify { get; set; }
    }
}
