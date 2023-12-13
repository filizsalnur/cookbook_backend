using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cookbook.Data;
using Cookbook.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cookbook.Pages
{
    public class RecipesModel : PageModel
    {
        private readonly RecipeService _recipeService;
        private readonly UserService _userService; 

        public RecipesModel(RecipeService recipeService, UserService userService)
        {
            _recipeService = recipeService;
            _userService = userService;
        }

        public List<Recipe> Recipes { get; set; }

        public string RecipeOwnerUsername { get; set; }

        public async Task OnGetAsync()
        {
            Recipes = await _recipeService.GetRecipes();

     
        }



        public async Task<IActionResult> OnGetRecipeDetailsAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _recipeService.GetRecipe(id);

            if (recipe == null)
            {
                return NotFound();
            }
            var user = await _userService.GetUser(recipe.UserId);

            if (user.UserName != null)
            {
                RecipeOwnerUsername = user.UserName;
            }
            else
            {
                RecipeOwnerUsername = "Unknown";
            }


            return Page();
        }

    }
}
