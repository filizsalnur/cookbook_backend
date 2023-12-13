using MongoDB.Bson.Serialization.Attributes;

namespace Cookbook.Data
{
    public class Recipe
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string UserId { get; set; }

        public string UserName { get; set; }
    }
}
