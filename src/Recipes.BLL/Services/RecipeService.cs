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
    private readonly ICurrentUser _currentUser;

    public RecipeService(RecipesContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<RecipeDto>> GetRecipes()
    {
        return await _context.Recipes.Include(r => r.Author)
            .Select(r => r.ToDto())
            .ToListAsync();
    }
    
    public async Task<RecipeDto> CreateRecipe(string description, string imageUrl)
    {
        var recipe = new Recipe
        {
            Description = description,
            CreatedAt = DateTime.UtcNow,
            AuthorId = _currentUser.Id,
            ImageUrl = imageUrl
        };

        _context.Recipes.Add(recipe);
        await _context.SaveChangesAsync();


        await _context.Entry(recipe).Reference(b => b.Author)
            .LoadAsync();
        
        return recipe.ToDto();
    }
}