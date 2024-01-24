namespace Cookbook.Infrastructur
{
    public static class SqliteCommandTexts //parameterize (create tables class, columns class)
    {
        public const string Create = $"""
                        INSERT INTO Recipes (Title, Author, NumberOfPortions, Calories, Slug, Guid)
                          VALUES (@Title, @Author, @NumberOfPortions, @Calories, @Slug, @Id);
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

        public const string GetAll = """
                        SELECT Recipes.Title, 
                               Recipes.Author, 
                               Recipes.NumberOfPortions, 
                               Recipes.Calories, 
                               Recipes.Slug, 
                               Recipes.Guid,
                               GROUP_CONCAT(Ingredients.Amount || ' ' || 
                                            Ingredients.Unit || ' ' || 
                                            Ingredients.Name, ', ') AS IngredientsList,
                               GROUP_CONCAT(Steps.Number || '. ' || 
                                            Steps.Description, ', ') AS StepsList,
                               GROUP_CONCAT(Tags.Description, ', ') AS TagsList
                        FROM Recipes
                        JOIN Ingredients ON Recipes.Slug = Ingredients.RecipeSlug
                        JOIN Steps ON Recipes.Slug = Steps.RecipeSlug
                        JOIN Tags ON Recipes.Slug = Tags.RecipeSlug
                        GROUP BY Recipes.Slug, Recipes.Title, Recipes.Author, 
                                        Recipes.NumberOfPortions, Recipes.Calories
            """;

        public const string GetBySlug = """
                        SELECT Recipes.Title, 
                               Recipes.Author, 
                               Recipes.NumberOfPortions, 
                               Recipes.Calories, 
                               Recipes.Slug, 
                               Recipes.Guid,
                               GROUP_CONCAT(Ingredients.Amount || ' ' || 
                                            Ingredients.Unit || ' ' || 
                                            Ingredients.Name, ', ') AS IngredientsList,
                               GROUP_CONCAT(Steps.Number || '. ' || 
                                            Steps.Description, ', ') AS StepsList,
                               GROUP_CONCAT(Tags.Description, ', ') AS TagsList
                        FROM Recipes
                        JOIN Ingredients ON Recipes.Slug = Ingredients.RecipeSlug
                        JOIN Steps ON Recipes.Slug = Steps.RecipeSlug
                        JOIN Tags ON Recipes.Slug = Tags.RecipeSlug
                        WHERE Recipes.Slug = @slug
                        GROUP BY Recipes.Slug, Recipes.Title, Recipes.Author, 
                                Recipes.NumberOfPortions, Recipes.Calories
            """;

        public static string GetById = """ 
                        SELECT Recipes.Title, 
                               Recipes.Author, 
                               Recipes.NumberOfPortions, 
                               Recipes.Calories, 
                               Recipes.Slug, 
                               Recipes.Guid,
                               GROUP_CONCAT(Ingredients.Amount || ' ' || 
                                            Ingredients.Unit || ' ' || 
                                            Ingredients.Name, ', ') AS IngredientsList,
                               GROUP_CONCAT(Steps.Number || '. ' || 
                                            Steps.Description, ', ') AS StepsList,
                               GROUP_CONCAT(Tags.Description, ', ') AS TagsList
                        FROM Recipes
                        JOIN Ingredients ON Recipes.Slug = Ingredients.RecipeSlug
                        JOIN Steps ON Recipes.Slug = Steps.RecipeSlug
                        JOIN Tags ON Recipes.Slug = Tags.RecipeSlug
                        WHERE Recipes.Guid = @id
                        GROUP BY Recipes.Slug, Recipes.Title, Recipes.Author, 
                                Recipes.NumberOfPortions, Recipes.Calories
            """;

        public const string ExistsById = @"SELECT COUNT(1) FROM Recipes WHERE Guid = @id";

        public const string ExistsBySlug = @"SELECT COUNT(1) FROM Recipes WHERE Slug = @slug";

        public const string GetAllTitles = @"SELECT Recipes.Title FROM Recipes";

        public const string GetAllTitlesWithTag = @"SELECT Recipes.Title FROM Recipes 
                                                    JOIN Tags ON Recipes.Slug = Tags.RecipeSlug
                                                    WHERE Tags.Description = @tag";
    }
}