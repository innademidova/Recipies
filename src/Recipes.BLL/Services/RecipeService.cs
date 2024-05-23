using Microsoft.EntityFrameworkCore;
using Recipes.BLL.DTOs.Recipe;
using Recipes.BLL.Exceptions;
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
        if (_currentUser.IsBanned)
        {
            throw new RecipesValidationException("You are banned");
        }

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

    public async Task<List<Comment>> GetComments(int recipeId)
    {
        return await _context.Comments
            .Include(p => p.AuthorId)
            .Where(c => c.RecipeId == recipeId)
            .ToListAsync();
    }

    public async Task<Comment> CreateComment(int recipeId, string text)
    {
        var newComment = new Comment
        {
            Text = text,
            AuthorId = _currentUser.Id,
            CreatedAt = DateTime.UtcNow,
            RecipeId = recipeId
        };

        await _context.Comments.AddAsync(newComment);
        await _context.SaveChangesAsync();
        
        return newComment;
    }
}