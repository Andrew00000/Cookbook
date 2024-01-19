namespace Cookbook.Infrastructur
{
    public static class SqliteCommandTexts
    {
        public const string InsertIntoRecipesTable = """
                        INSERT INTO Recipes (Title, Author, Portions, Calories, Slug, Guid)
                          VALUES (@Title, @Author, @Portions, @Calories, @Slug, @Id);
                        """;
        public const string InsertIntoRecipesIngredients = """
                        INSERT INTO Ingredients ( RecipeSlug, Name, Amount, Unit)
                          VALUES (@Slug, @Name, @Amount, @Unit);
                        """;
        public const string InsertIntoRecipesSteps = """
                        INSERT INTO Steps ( RecipeSlug, Number, Description)
                          VALUES (@Slug, @Number, @Description);
                        """;
        public const string InsertIntoRecipesTags = """
                        INSERT INTO Tags ( RecipeSlug, Description)
                          VALUES (@Slug, @Description);
                        """;
    }
}
