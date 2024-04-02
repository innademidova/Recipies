using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.DAL;
using Recipes.DAL.Models;
using Recipes.Extensions;
using Recipes.ViewModel;

namespace Recipes.Controllers;

[Route("[controller]")]
[ApiController]
public class RecipesController(RecipesContext context) : ControllerBase
{
	[HttpGet]
	public async Task<IEnumerable<Recipe>> Get()
	{
		return await context.Recipes.Include(r => r.Author).ToListAsync();
	}

	[Authorize] 
	[HttpPost]
	public async Task<Recipe> Create(CreateRecipeRequest request)
	{
		var id = this.GetUserId();
		var recipe = new Recipe
		{
			Description = request.Description,
			CreatedAt = DateTime.UtcNow,
			AuthorId = id,
			ImageUrl = request.ImageUrl
		};
		
		context.Recipes.Add(recipe);
		await context.SaveChangesAsync();
		return recipe;
	}
}