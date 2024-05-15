using Recipes.BLL.Interfaces;
using Recipes.DAL.Models;

namespace Recipes.Infrastructure;

internal class CurrentUser : ICurrentUser
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public UserRole Role { get; set; }
    public bool IsBanned { get; set; }
}