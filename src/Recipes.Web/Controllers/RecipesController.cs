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
    [AllowAnonymous]
    public async Task<IEnumerable<RecipeViewModel>> Get(int pageSize = 5, int pageNumber = 1)
    {
        var recipes = await _recipeService.GetRecipes(pageSize, pageNumber);
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
    [AllowAnonymous]
    public async Task<IEnumerable<Comment>> GetComments(int recipeId, int pageSize = 5, int pageNumber = 1)
    {
        return await _recipeService.GetComments(recipeId, pageSize, pageNumber);
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
    
    [HttpPut("{recipeId:int}/revomefavorite")]
    public async Task<int> RemoveFromFavorite(int recipeId)
    {
        return await _recipeService.RemoveFromFavorite(recipeId);
    }
}