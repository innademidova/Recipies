using Bogus;
using Recipes.ViewModel;
using Recipes.ViewModels;
using Swashbuckle.AspNetCore.Filters;

namespace Recipes.SwaggerExamples.User;

public class CreateRecipeExamples : IMultipleExamplesProvider<CreateRecipeRequest>
{
    public IEnumerable<SwaggerExample<CreateRecipeRequest>> GetExamples()
    {
        return Enumerable.Range(0, 10)
            .Select(number => SwaggerExample.Create($"Example {number}", GetExampleWithBogus()))
            .ToList();
    }

    private CreateRecipeRequest GetExampleWithBogus()
    {
        var faker = new Faker();

        return new CreateRecipeRequest(faker.WaffleText(), "url");
    }
}