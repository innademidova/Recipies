using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.BLL.Authentication;
using Recipes.DAL;
using Recipes.DAL.Models;
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
	public async Task<ActionResult<User>> Register(RegistrationRequest request)
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
			
			context.Users.Add(user);
			await context.SaveChangesAsync();

			var accessToken = _tokenGenerator.GenerateToken(user);
			return Ok(new RegistrationResponse(user.Id, accessToken));
		}
		catch (Exception e)
		{
			return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "An error occurred while processing your request.");
		}
	}

	[HttpPost("login")]
	public async Task<ActionResult<User>> Login(LoginRequest request)
	{
		var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

		if (user == null)
		{
			return BadRequest("Email or password are incorrect");
		}
		
		if (user.Password != request.Password)
		{
			return BadRequest("Email or password are incorrect");
		}

		var accessToken = _tokenGenerator.GenerateToken(user);
		
		return Ok(new RegistrationResponse(user.Id, accessToken));
	}
}