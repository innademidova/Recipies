﻿using Recipes.BLL.DTOs.Recipe;
using Recipes.DAL.Models;

namespace Recipes.BLL.Mappers;

public static class RecipeMapper
{
    public static RecipeDto ToDto(this Recipe r, int currentUserId)
    {
        return new RecipeDto(r.Id, r.Description, r.CreatedAt,
            r.Author.FirstName, r.Author.LastName, r.Author.Id, r.ImageUrl,
            r.Favorites.Count, r.Favorites.Any(f => f.UserId == currentUserId), r.Comments.Count);
    }
}