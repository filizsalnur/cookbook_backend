using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Cookbook.Data
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<Recipe> Recipes { get; set; }
    }
}