using System.Text.RegularExpressions;

namespace Cookbook.Domain.Models
{
    public partial class Recipe
    {
        public long Id { get; set; }
        public required string Title { get; init; }
        public required string Author { get; init; }
        public required int NumberOfPortions { get; init; }
        public int Calories { get; set; } = 0;
        public required IEnumerable<Ingredient> Ingredients { get; init; }
        public required IEnumerable<string> Steps { get; init; }
        public IEnumerable<string> Tags { get; init; } = Enumerable.Empty<string>();
        public string Slug => GenerateSlug();

        private string GenerateSlug()
        {
            var sluggedTitle = SlugRegex().Replace(Title, string.Empty).ToLower().Replace(" ", "-");
            var sluggedAuthor = SlugRegex().Replace(Author, string.Empty).ToLower().Replace(" ", "-");

            return $"{sluggedTitle}-{sluggedAuthor}";
        }

        [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
        private static partial Regex SlugRegex();
    }
}
