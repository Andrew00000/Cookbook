namespace Cookbook.Contracts.Responses
{
    public class RecipesResponse
    {
        public required IEnumerable<RecipeResponse> Recipes { get; init; } = 
                                            Enumerable.Empty<RecipeResponse>();
    }
}
