namespace Recipes.Authentication;

public static class AuthOptions
{
    public const string Issuer= "Recipes";
    public const string Audience= "http://localhost:5000/";
    public const string Key= "mysupersecret_secretkey!123_for#RecipesApplicationAddedTextForTest";
    public static readonly TimeSpan Expiration = TimeSpan.FromMinutes(30);
}