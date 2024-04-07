using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.BLL.Authentication;
using Recipes.BLL.Services;
using Recipes.DAL;
using Recipes.DAL.Models;
using Recipes.ViewModel;

namespace Recipes.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController(RecipesContext context, UserService userService) : ControllerBase
{
    private readonly JwtTokenGenerator _tokenGenerator = new();

    [HttpGet]
    public async Task<IEnumerable<User>> Get()
    {
        return await context.Users.ToListAsync();
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(RegistrationRequest request)
    {
        var existedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (existedUser != null)
        {
            throw new Exception("Unhandled exception: user already exists");
        }

        var user = new User()
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

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login(LoginRequest request)
    {
        var loginResponse = await userService.Login(request.Email, request.Password);

        return Ok(new RegistrationResponse(loginResponse.UserId, loginResponse.AccessToken));
    }
}