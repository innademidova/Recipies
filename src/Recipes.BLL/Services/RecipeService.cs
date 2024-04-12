using Microsoft.EntityFrameworkCore;
using Recipes.BLL.DTOs.Recipe;
using Recipes.BLL.Interfaces;
using Recipes.BLL.Mappers;
using Recipes.DAL;
using Recipes.DAL.Models;

namespace Recipes.BLL.Services;

public class RecipeService : IRecipeService
{
    private readonly RecipesContext _context;

    public RecipeService(RecipesContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RecipeDto>> GetRecipes()
    {
        return await _context.Recipes.Include(r => r.Author)
            .Select(r => r.ToDto())
            .ToListAsync();
    }
    
    public async Task<RecipeDto> CreateRecipe(string description, int userId, string imageUrl)
    {
        var recipe = new Recipe
        {
            Description = description,
            CreatedAt = DateTime.UtcNow,
            AuthorId = userId,
            ImageUrl = imageUrl
        };

        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();

        return recipe.ToDto();
    }
}