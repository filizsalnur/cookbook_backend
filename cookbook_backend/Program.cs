using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Cookbook.Data;
using AdressAPI.Data;
using Cookbook.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AddressesDatabaseSettings>(builder.Configuration.GetSection("AddressesDatabaseSettings"));
builder.Services.Configure<UserDatabaseSettings>(builder.Configuration.GetSection("UserDatabaseSettings"));
builder.Services.Configure<RecipeDatabaseSettings>(builder.Configuration.GetSection("RecipeDatabaseSettings"));


builder.Services.AddSingleton<AddressesService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<RecipeService>();
var app = builder.Build();


app.MapGet("/", () => "Address API");


app.MapGet("/api/Address", async (AddressesService addressesService) => await addressesService.Get());


app.MapGet("/api/Address/{id}", async (AddressesService addressesService, string id) =>
{
    var address = await addressesService.Get(id);
    return address is null ? Results.NotFound() : Results.Ok(address);
});


app.MapPost("/api/Address", async (AddressesService addressesService, Address address) =>
{
    await addressesService.Create(address);
    return Results.Ok();
});


app.MapPut("/api/Address/{id}", async (AddressesService addressesService, string id, Address updatedAddress) =>
{
    var address = await addressesService.Get(id);
    if (address is null) return Results.NotFound();

    updatedAddress.Id = address.Id;
    await addressesService.Update(id, updatedAddress);

    return Results.NoContent();
});


app.MapDelete("/api/Address/{id}", async (AddressesService addressesService, string id) =>
{
    var address = await addressesService.Get(id);
    if (address is null) return Results.NotFound();

    await addressesService.Remove(address.Id);

    return Results.NoContent();
});

// Users Endpoint
app.MapGet("/api/User", async (UserService userService) => await userService.GetUsers());
app.MapGet("/api/User/{id}", async (UserService userService, string id) =>
{
    var user = await userService.GetUser(id);
    return user is null ? Results.NotFound() : Results.Ok(user);
});
app.MapPost("/api/User", async (UserService userService, User newUser) =>
{
    await userService.CreateUser(newUser);
    return Results.Ok();
});
app.MapPut("/api/User/{id}", async (UserService userService, string id, User updatedUser) =>
{
    var user = await userService.GetUser(id);
    if (user is null) return Results.NotFound();

    updatedUser.Id = user.Id;
    await userService.UpdateUser(id, updatedUser);

    return Results.NoContent();
});
app.MapDelete("/api/User/{id}", async (UserService userService, string id) =>
{
    var user = await userService.GetUser(id);
    if (user is null) return Results.NotFound();

    await userService.RemoveUser(user.Id);

    return Results.NoContent();
});


// Recipes Endpoint
app.MapGet("/api/Recipe", async (RecipeService recipeService) => await recipeService.GetRecipes());
app.MapGet("/api/Recipe/{id}", async (RecipeService recipeService, string id) =>
{
    var recipe = await recipeService.GetRecipe(id);
    return recipe is null ? Results.NotFound() : Results.Ok(recipe);
});
app.MapPost("/api/Recipe", async (RecipeService recipeService, Recipe newRecipe) =>
{
    await recipeService.CreateRecipe(newRecipe);
    return Results.Ok();
});
app.MapPut("/api/Recipe/{id}", async (RecipeService recipeService, string id, Recipe updatedRecipe) =>
{
    var recipe = await recipeService.GetRecipe(id);
    if (recipe is null) return Results.NotFound();

    updatedRecipe.Id = recipe.Id;
    await recipeService.UpdateRecipe(id, updatedRecipe);

    return Results.NoContent();
});
app.MapDelete("/api/Recipe/{id}", async (RecipeService recipeService, string id) =>
{
    var recipe = await recipeService.GetRecipe(id);
    if (recipe is null) return Results.NotFound();

    await recipeService.RemoveRecipe(recipe.Id);

    return Results.NoContent();
});


app.Run();
