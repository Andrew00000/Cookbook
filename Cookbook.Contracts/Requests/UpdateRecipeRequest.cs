namespace Cookbook.Contracts.Requests
{
    public class UpdateRecipeRequest
    {
        public required string Title { get; init; }
        public string Author { get; init; } = string.Empty;
        public required int NumberOfPortions { get; init; }
        public int Calories { get; init; } = 0;
        public required IEnumerable<string> Ingredients { get; init; }
        public required IEnumerable<string> Steps { get; init; }
        public IEnumerable<string> Tags { get; init; } = Enumerable.Empty<string>();
    }
}
