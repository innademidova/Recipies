using Recipes.BLL.DTOs.Recipe;
using Recipes.DAL.Models;

namespace Recipes.BLL.Interfaces;

public interface IRecipeService
{
    Task<IEnumerable<RecipeDto>> GetRecipes();
    Task<RecipeDto> CreateRecipe(string description, string imageUrl);
    Task<List<Comment>> GetComments(int postId);
    Task<Comment> CreateComment(int recipeId, string text);
}