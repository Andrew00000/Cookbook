namespace Cookbook.Domain.Models
{
    public class Ingredient
    {
        public required int Amount { get; init; }
        public required string Unit { get; init; }
        public required string Name { get; init; }
    }
}
