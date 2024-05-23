using System.ComponentModel.DataAnnotations;

namespace Recipes.ViewModels;

public record CreateCommentRequest([MaxLength(1000)] string Text);