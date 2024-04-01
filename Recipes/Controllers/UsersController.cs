using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.Authentication;
using Recipes.Models;
using Recipes.ViewModel;

namespace Recipes.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController(RecipesContext context): ControllerBase
{
	private readonly JwtTokenGenerator _tokenGenerator = new();
	
	[HttpGet]
	public async Task<IEnumerable<User>> Get()
	{
		return await context.Users.ToListAsync();
	}

	[HttpPost]
	public async Task<ActionResult<User>> Create(CreateUserRequest request)
	{
		var existedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

		if (existedUser != null)
		{
			return BadRequest($"Email {request.Email} has already exist");
		}
		
		try
		{
			User user = new User()
			{
				Email = request.Email,
				CreatedAt = DateTime.UtcNow,
				FirstName = request.FirstName,
				LastName = request.LastName,
				Password = request.Password
			};
			
			var accessToken = _tokenGenerator.GenerateToken(user);
			context.Users.Add(user);
			await context.SaveChangesAsync();

			return Ok(new{accessToken, user});
		}
		catch (Exception e)
		{
			return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "An error occurred while processing your request.");
		}
	}
}