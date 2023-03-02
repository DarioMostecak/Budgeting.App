using Xunit;

namespace Budgeting.App.Api.Tests.Acceptance.Brokers
{
    [CollectionDefinition(nameof(ApiTestCollection))]
    public class ApiTestCollection : ICollectionFixture<BudgetingAppApiBroker>
    {
    }
}
