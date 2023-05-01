namespace Budgeting.Web.App.Brokers.UniqueIDGenerators
{
    public class UniqueIDGeneratorBroker : IUniqueIDGeneratorBroker
    {
        public Guid GenerateUniqueID() =>
            Guid.NewGuid();
    }
}
