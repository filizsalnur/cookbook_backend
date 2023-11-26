using Cookbook.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cookbook.Service
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IOptions<UserDatabaseSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);

            _users = mongoClient.GetDatabase(options.Value.DatabaseName)
                .GetCollection<User>(options.Value.UserCollectionName);
        }

        public async Task<List<User>> GetUsers() =>
            await _users.Find(_ => true).ToListAsync();

        public async Task<User> GetUser(string userId) =>
            await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();

        public async Task CreateUser(User newUser) =>
            await _users.InsertOneAsync(newUser);

        public async Task AddRecipeToUser(string userId, Recipe newRecipe)
        {
            var userFilter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.Push(u => u.Recipes, newRecipe);
            await _users.UpdateOneAsync(userFilter, update);
        }

        public async Task UpdateUser(string userId, User updatedUser) =>
            await _users.ReplaceOneAsync(u => u.Id == userId, updatedUser);

        public async Task RemoveUser(string userId) =>
            await _users.DeleteOneAsync(u => u.Id == userId);
    }
}
