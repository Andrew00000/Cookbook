namespace Cookbook.Domain
{
    public class Ingredient
    {
        public required int Volume { get; init; }
        public required UnitType Unit { get; init; }
        public required string Name { get; init; }
    }
}
