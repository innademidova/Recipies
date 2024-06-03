using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.BLL.Interfaces;
using Recipes.DAL;
using Recipes.DAL.Models;
using Recipes.Mappers;
using Recipes.ViewModels;

namespace Recipes.Controllers;

[Route("[controller]")]
[ApiController]
public class RecipesController(RecipesContext context, ILogger<RecipesController> _logger, IRecipeService _recipeService) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<RecipeViewModel>> Get()
    {
        var recipes = await _recipeService.GetRecipes();
        var recipesViewModel = recipes.Select(r =>
            r.ToViewModel());

        return recipesViewModel;
    }
    
    [HttpPost]
    public async Task<RecipeViewModel> Create(CreateRecipeRequest request)
    {
        var recipeDto = await _recipeService.CreateRecipe(request.Description, request.ImageUrl);

        _logger.LogInformation("Created recipe by Author {AuthorId}", recipeDto.AuthorId);

        return recipeDto.ToViewModel();
    }
    
    [HttpGet("{recipeId:int}/comments")]
    public async Task<IEnumerable<Comment>> GetComments(int postId)
    {
        return await _recipeService.GetComments(postId);
    }
    
    [HttpPost("{recipeId:int}/comments")]
    [Authorize]
    public async Task<Comment> CreateComment(int recipeId, CreateCommentRequest request)
    {
        return await _recipeService.CreateComment(recipeId, request.Text);
    }
    
    [HttpPut("{recipeId:int}/favorite")]
    public async Task<int> AddToFavorite(int recipeId)
    {
        return await _recipeService.AddToFavorite(recipeId);
    }
}