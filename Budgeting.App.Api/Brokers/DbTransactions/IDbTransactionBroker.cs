// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

namespace Budgeting.App.Api.Brokers.DbTransactions
{
    public interface IDbTransactionBroker
    {
        void BeginTransaction();
        void CommitTransaction();
        void RollBackTransaction();
        void DisposeTransaction();
    }
}
