using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.Models;
using Recipes.ViewModel;

namespace Recipes.Controllers;

[Route("[controller]")]
[ApiController]
public class RecipesController(RecipesContext context) : ControllerBase
{
	[HttpGet]
	public async Task<IEnumerable<Recipe>> Get()
	{
		return await context.Recipes.ToListAsync();
	}

	[HttpPost]
	public async Task<Recipe> Create(CreateRecipeRequest request)
	{
		var recipe = new Recipe
		{
			Description = request.Description,
			CreatedAt = DateTime.UtcNow,
			Author = request.Author,
			ImageUrl = request.ImageUrl
		};
		
		context.Recipes.Add(recipe);
		await context.SaveChangesAsync();
		return recipe;
	}
}