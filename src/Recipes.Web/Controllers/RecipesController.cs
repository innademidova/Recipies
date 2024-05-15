using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.BLL.Interfaces;
using Recipes.DAL;
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
}