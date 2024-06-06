using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipes.BLL.DTOs.User;
using Recipes.BLL.Services;
using Recipes.DAL;
using Recipes.DAL.Models;
using Recipes.Extensions;
using Recipes.ViewModel;
using Recipes.ViewModels;

namespace Recipes.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController(RecipesContext context, UserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<User>> Get()
    {
        return await userService.GetUsers();
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
    {
        var userResponse = await userService.Register(request.FirstName, request.LastName, request.Email, request.Password);

        return this.ToOk(userResponse, result => new RegistrationResponse(result.UserId, result.AccessToken));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<RegistrationResponse>> Login(LoginRequest request)
    {
        var loginResponse = await userService.Login(request.Email, request.Password);

        return this.ToOk(loginResponse, result => new RegistrationResponse(result.UserId, result.AccessToken));
    }
    
    [HttpPut("ban/{userId:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task BanUser(int userId)
    {
        await userService.Ban(userId);
    }
    
    [HttpPut("subscribe/{userId:int}")]
    [Authorize]
    public async Task Subscribe(int userId)
    {
        await userService.Subscribe(userId);
    }
    
    [Authorize]
    [HttpPut("unsubscribe/{userId:int}")]
    public async Task Unsubscribe(int userId)
    {
        await userService.Unsubscribe(userId);
    }
    
    [Authorize]
    [HttpGet("subscriptions")]
    public async Task<List<UserSubscription>> GetSubscriptions()
    {
        return await userService.GetSubscriptions();
    }
    
    [Authorize]
    [HttpGet("subscribers")]
    public async Task<List<UserSubscription>> GetSubscribers()
    {
        return await userService.GetSubscribers();
    }
    
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<AccountDetailsDto>> GetMyInfo()
    {
        return await userService.GetCurrentAccountDetail();
    }
}