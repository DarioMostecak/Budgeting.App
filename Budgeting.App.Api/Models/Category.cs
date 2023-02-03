using Budgeting.App.Api.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Budgeting.App.Api.Models
{
    [BsonCollection("categories")]
    public class Category
    {
        private Category() { }

        [BsonId]
        [BsonElement("_categoryId")]
        [BsonRepresentation(BsonType.String)]
        public Guid CategoryId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("icon")]
        public string Icon { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("time_created")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime TimeCreated { get; set; }

        [BsonElement("time_modify")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime TimeModify { get; set; }

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
    }
}
