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
        private readonly RecipeService _recipeService;

        public UserService(IOptions<UsersDatabaseSettings> options, RecipeService recipeService)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            _users = mongoClient.GetDatabase(options.Value.DatabaseName)
                .GetCollection<User>(options.Value.UsersCollectionName);

            _recipeService = recipeService;
        }

        public async Task<List<User>> GetUsers() =>
            await _users.Find(_ => true).ToListAsync();

        public async Task<User> GetUser(string userId) =>
            await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();

        public async Task CreateUser(User newUser, string password)
        {
            newUser.Password = password;

            foreach (var recipe in newUser.Recipes)
            {
                await _recipeService.CreateRecipe(recipe,newUser.Id);
            }

            await _users.InsertOneAsync(newUser);
        }

        public async Task AddRecipeToUser(string userId, Recipe newRecipe)
        {
            var userFilter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.Push(u => u.Recipes, newRecipe);
            await _users.UpdateOneAsync(userFilter, update);

            await _recipeService.CreateRecipe(newRecipe, userId);
        }

        public async Task UpdateUser(string userId, User updatedUser) =>
            await _users.ReplaceOneAsync(u => u.Id == userId, updatedUser);

        public async Task RemoveUser(string userId) =>
            await _users.DeleteOneAsync(u => u.Id == userId);

        public async Task<User> GetUserByEmail(string email) =>
           await _users.Find(u => u.Email == email).FirstOrDefaultAsync();

        public bool VerifyPassword(User user, string password)
        {
            return user.Password == password;
        }

        public async Task<(string UserName, string UserId)> GetUserByRecipeId(string recipeId)
        {
            try
            {
                Console.WriteLine($"Searching for user with recipeId: {recipeId}");

                var user = await _users.Find(u => u.Recipes.Any(r => r.Id == recipeId)).FirstOrDefaultAsync();

                if (user != null)
                {
                    Console.WriteLine($"Found user: {user.UserName}, {user.Id}");
                    return (user.UserName, user.Id);
                }
                else
                {
                    Console.WriteLine("User not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            return (null, null);
        }
    }
}
