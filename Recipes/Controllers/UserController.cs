using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.Models;
using Recipes.ViewModel;

namespace Recipes.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController(RecipesContext context): ControllerBase
{
	[HttpGet]
	public async Task<IEnumerable<User>> Get()
	{
		return await context.Users.ToListAsync();
	}

	[HttpPost]
	public async Task<ActionResult<User>> Create(CreateUserRequest request)
	{
		try
		{
			User user = new User()
			{
				Email = request.Email,
				CreatedAt = DateTime.UtcNow,
				FirstName = request.FirstName,
				LastName = request.LastName
			};

			context.Users.Add(user);
			await context.SaveChangesAsync();

			return user;
		}
		catch (Exception e)
		{
			return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "An error occurred while processing your request.");
		}
	}
}