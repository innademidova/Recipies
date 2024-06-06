using Recipes.BLL.DTOs.Recipe;
using Recipes.DAL.Models;

namespace Recipes.BLL.Interfaces;

public interface IRecipeService
{
    Task<IEnumerable<RecipeDto>> GetRecipes(int pageSize, int pageNumber);
    Task<RecipeDto> CreateRecipe(string description, string imageUrl);
    Task<List<Comment>> GetComments(int postId, int pageSize, int pageNumber);
    Task<Comment> CreateComment(int recipeId, string text);
    Task<int> AddToFavorite(int recipeId);
    Task<int> RemoveFromFavorite(int recipeId);
}