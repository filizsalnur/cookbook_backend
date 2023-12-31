﻿using Cookbook.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cookbook.Service
{
    public class RecipeService
    {
        private readonly IMongoCollection<Recipe> _recipes;
        private readonly IMongoCollection<User> _users;

        public RecipeService(IOptions<RecipesDatabaseSettings> options, IOptions<UsersDatabaseSettings> usersOptions)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);
            var usersMongoClient = new MongoClient(usersOptions.Value.ConnectionString);

            _recipes = mongoClient.GetDatabase(options.Value.DatabaseName)
                .GetCollection<Recipe>(options.Value.RecipesCollectionName);

            _users = usersMongoClient.GetDatabase(usersOptions.Value.DatabaseName)
                .GetCollection<User>(usersOptions.Value.UsersCollectionName);
        }

        public async Task<List<Recipe>> GetRecipes() =>
            await _recipes.Find(_ => true).ToListAsync();

        public async Task<Recipe> GetRecipe(string recipeId) =>
            await _recipes.Find(r => r.Id == recipeId).FirstOrDefaultAsync();

        public async Task CreateRecipe(Recipe newRecipe, string userId)
        {
            newRecipe.UserId = userId;


            var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            if (user != null)
            {
                newRecipe.UserName = user.UserName;
            }

            await _recipes.InsertOneAsync(newRecipe);

    
            var userFilter = Builders<User>.Filter.Eq(u => u.Id, userId);
            var update = Builders<User>.Update.Push(u => u.Recipes, newRecipe);
            await _users.UpdateOneAsync(userFilter, update);
        }

        public async Task UpdateRecipe(string recipeId, Recipe updatedRecipe) =>
            await _recipes.ReplaceOneAsync(r => r.Id == recipeId, updatedRecipe);

        public async Task RemoveRecipe(string recipeId) =>
            await _recipes.DeleteOneAsync(r => r.Id == recipeId);
        
    }

}
