using Recipes.DAL.Models;

namespace Recipes.BLL.Interfaces;

public interface ICurrentUser
{
    int Id { get; set; }
    string Email { get; set; }
    UserRole Role { get; set; }
    bool IsBanned { get; set; }
}