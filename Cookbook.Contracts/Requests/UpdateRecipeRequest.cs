using Cookbook.Domain.Models;

namespace Cookbook.Contracts.Requests
{
    public class UpdateRecipeRequest
    {
        public required string Title { get; init; }
        public required string Author { get; init; }
        public required int NumberOfPortions { get; init; }
        public int Calories { get; init; } = 0;
        public required IEnumerable<Ingredient> Ingredients { get; init; }
        public required IEnumerable<string> Steps { get; init; }
        public IEnumerable<string> Tags { get; init; } = Enumerable.Empty<string>();
    }
}
