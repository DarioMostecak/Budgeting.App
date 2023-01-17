using Budgeting.Web.App.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Budgeting.Web.App.Models
{
    [BsonCollection("categories")]
    public sealed class Category
    {
        private Category() { }

        [BsonId]
        [BsonElement("_categoryId")]
        [BsonRepresentation(BsonType.String)]
        public Guid CategoryId { get; private set; }

        [BsonElement("title")]
        public string Title { get; private set; }

        [BsonElement("icon")]
        public string Icon { get; private set; }

        [BsonElement("type")]
        public string Type { get; private set; }

        [BsonElement("time_created")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime TimeCreated { get; private set; }

        [BsonElement("time_modify")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime TimeModify { get; private set; }



        public static Category CreateNewCategory(Guid CategoryId, string title,
            string icon, string type, DateTime timeCreated, DateTime timeModify)
        {
            return new Category
            {
                CategoryId = CategoryId,
                Title = title,
                Icon = icon,
                Type = type,
                TimeCreated = timeCreated,
                TimeModify = timeModify
            };
        }


        #region Public methods
        public void SetTimeCreatedAndTimeModify()
        {
            TimeCreated = DateTime.UtcNow;
            TimeModify = DateTime.UtcNow;
        }

        public void UpdateTimeModify()
        {
            TimeModify = DateTime.UtcNow;
        }
        #endregion
    }
}
