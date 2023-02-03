namespace Budgeting.App.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonCollectionAttribute : Attribute
    {
        private string collectionName;
        public BsonCollectionAttribute(string collectionName)
        {
            this.collectionName = collectionName;
        }

        public string CollectionName => this.collectionName;
    }
}
