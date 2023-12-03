using Cookbook.Data;
using Cookbook.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<UsersDatabaseSettings>(builder.Configuration.GetSection("UsersDatabaseSettings"));
builder.Services.Configure<RecipesDatabaseSettings>(builder.Configuration.GetSection("RecipesDatabaseSettings"));
builder.Services.AddRazorPages();

builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<RecipeService>();
var app = builder.Build();




// Kullanıcı Endpointleri
app.MapGet("/api/User", async (UserService userService) => await userService.GetUsers());
app.MapGet("/api/User/{id}", async (UserService userService, string id) =>
{
    var user = await userService.GetUser(id);
    return user is null ? Results.NotFound() : Results.Ok(user);
});
app.MapPost("/api/User", async (UserService userService, User newUser) =>
{
    await userService.CreateUser(newUser,newUser.Password);
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

// Tarif Endpointleri
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


app.MapRazorPages();

app.Run();
