using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Recipes.BLL.Authentication;
using Recipes.BLL.Exceptions;
using Recipes.BLL.Interfaces;
using Recipes.DAL;
using Recipes.DAL.Models;

namespace Recipes.BLL.Services;

public class UserService
{
    private readonly JwtTokenGenerator _tokenGenerator = new();
    private readonly RecipesContext _context;
    private readonly ICurrentUser _currentUser;

    public UserService(RecipesContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _context.Users.Include(u => u.UserAccount).AsNoTracking().ToListAsync();
    }
    public async Task<Result<(int UserId, string AccessToken)>> Register(string firstName, string lastName, string email, string password)
    {
        var existedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (existedUser != null)
        {
            return new Result<(int UserId, string AccessToken)>(new RecipesValidationException("Unhandled exception: user already exists"));
        }
        
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User
        {
            Email = email,
            CreatedAt = DateTime.UtcNow,
            FirstName = firstName,
            LastName = lastName,
            UserAccount = new UserAccount
            {
                Password = passwordHash,
            },
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var accessToken = _tokenGenerator.GenerateToken(user);

        return (user.Id, accessToken);
    }
    public async Task<Result<(int UserId, string AccessToken)>> Login(string email, string password)
    {
        var user = await _context.Users.Include(u => u.UserAccount).FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            return new Result<(int UserId, string AccessToken)>(new RecipesValidationException("Email or password are incorrect"));
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.UserAccount.Password))
        {
            return new Result<(int UserId, string AccessToken)>(new RecipesValidationException("Email or password are incorrect"));
        }

        var accessToken = _tokenGenerator.GenerateToken(user);

        return (user.Id, accessToken);
    }

    public async Task Ban(int userId)
    {
        var user = await _context.Users.Include(u => u.UserAccount).FirstOrDefaultAsync(u => u.Id == userId);
       

        if (user == null)
        {
            throw new RecipesValidationException("User doesn't exist.");
        }

        if (user.UserAccount.Role > _currentUser.Role)
        {
            throw new RecipesValidationException("You don't have permission on this action!");
        }

        user.UserAccount.IsBanned = true;
        await _context.SaveChangesAsync();
    }

    public async Task Subscribe(int userId)
    {
        var userToFollow = await _context.Users.FindAsync(userId);

        if (userToFollow == null)
        {
            throw new RecipesValidationException("User doesn't exist");
        }

        var isAlreadySubscribed = _context.UserSubscriptions.AnyAsync(s => s.SubscriberId == _currentUser.Id && s.SubscriptionId == userId);

        if (isAlreadySubscribed.Result)
        {
            return;
        }
        
        await _context.UserSubscriptions.AddAsync(new UserSubscription
        {
            SubscriberId = _currentUser.Id,
            SubscriptionId = userId,
            SubscribedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }

    public async Task Unsubscribe(int userId)
    {
        var subscriptionToUnsubscribe = await _context.UserSubscriptions.FirstOrDefaultAsync(s => s.SubscriberId == _currentUser.Id && s.SubscriptionId == userId);
        
        if (subscriptionToUnsubscribe != null)
        {
            _context.UserSubscriptions.Remove(subscriptionToUnsubscribe);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<List<UserSubscription>> GetSubscriptions()
    {
        return await _context.UserSubscriptions.Include(u => u.Subscription).Where(s => s.SubscriberId == _currentUser.Id).ToListAsync();
    }
    
    public async Task<List<UserSubscription>> GetSubscribers()
    {
        return await _context.UserSubscriptions.Include(u => u.Subscriber).Where(s => s.SubscriptionId == _currentUser.Id).ToListAsync();
    }
}