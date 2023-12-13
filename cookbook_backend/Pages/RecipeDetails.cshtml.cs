using System.Threading.Tasks;
using Cookbook.Data;
using Cookbook.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cookbook.Pages
{
    public class RecipeDetailsModel : PageModel
    {
        private readonly RecipeService _recipeService;

        public RecipeDetailsModel(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public Recipe Recipe { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Recipe = await _recipeService.GetRecipe(id);

            if (Recipe == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
