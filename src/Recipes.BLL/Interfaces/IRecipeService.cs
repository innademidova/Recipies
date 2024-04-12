using Recipes.BLL.DTOs.Recipe;

namespace Recipes.BLL.Interfaces;

public interface IRecipeService
{
    Task<IEnumerable<RecipeDto>> GetRecipes();
    Task<RecipeDto> CreateRecipe(string description, int userId, string imageUrl);
}