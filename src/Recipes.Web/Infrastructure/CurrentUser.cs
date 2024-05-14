using Recipes.BLL.Interfaces;

namespace Recipes.Infrastructure;

internal class CurrentUser : ICurrentUser
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
}