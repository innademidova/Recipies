using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Recipes.BLL.Services;
using Recipes.DAL;
using Recipes.DAL.Models;
using Recipes.Extensions;
using Recipes.ViewModel;

namespace Recipes.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController(RecipesContext context, UserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<User>> Get()
    {
        return await context.Users.ToListAsync();
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
    {
        var userResponse = await userService.Register(request.FirstName, request.LastName, request.Email, request.Password);

        return this.ToOk(userResponse, result => new RegistrationResponse(result.UserId, result.AccessToken));
    }

    [HttpPost("login")]
    public async Task<ActionResult<RegistrationResponse>> Login(LoginRequest request)
    {
        var loginResponse = await userService.Login(request.Email, request.Password);

        return this.ToOk(loginResponse, result => new RegistrationResponse(result.UserId, result.AccessToken));
    }
}