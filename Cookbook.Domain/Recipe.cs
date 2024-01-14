namespace Cookbook.Domain
{
    public class Recipe
    {
        public required Guid Id { get; init; }
        public required string Title { get; set; }
        public string Author { get; set; } = string.Empty;
        public required int Portions { get; set; }
        public int Calories { get; set; } = 0;
        public required IEnumerable<Ingredient> Ingredients { get; init; }
        public required IEnumerable<string> Steps { get; init; }
        public IEnumerable<string> Tags { get; init; } = Enumerable.Empty<string>();
    }
}
