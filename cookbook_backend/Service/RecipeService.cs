using Cookbook.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cookbook.Service
{
    public class RecipeService
    {
        private readonly IMongoCollection<Recipe> _recipes;

        public RecipeService(IOptions<RecipeDatabaseSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);

            _recipes = mongoClient.GetDatabase(options.Value.DatabaseName)
                .GetCollection<Recipe>(options.Value.RecipeCollectionName);
        }

        public async Task<List<Recipe>> GetRecipes() =>
            await _recipes.Find(_ => true).ToListAsync();

        public async Task<Recipe> GetRecipe(string recipeId) =>
            await _recipes.Find(r => r.Id == recipeId).FirstOrDefaultAsync();

        public async Task CreateRecipe(Recipe newRecipe) =>
            await _recipes.InsertOneAsync(newRecipe);

        public async Task UpdateRecipe(string recipeId, Recipe updatedRecipe) =>
            await _recipes.ReplaceOneAsync(r => r.Id == recipeId, updatedRecipe);

        public async Task RemoveRecipe(string recipeId) =>
            await _recipes.DeleteOneAsync(r => r.Id == recipeId);
    }
}
