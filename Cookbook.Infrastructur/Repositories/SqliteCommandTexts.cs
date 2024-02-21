namespace Cookbook.Repository.Repositories
{
    public static class SqliteCommandTexts //parameterize (create tables class, columns class)
    {
        public const string Create = $"""
                        INSERT INTO Recipes (Title, Author, NumberOfPortions, Calories, Slug, Guid)
                          VALUES (@Title, @Author, @NumberOfPortions, @Calories, @Slug, @Id);
                        """;

        public const string InsertIntoRecipesIngredients = """
                        INSERT INTO Ingredients ( RecipeId, Name, Amount, Unit)
                          VALUES (@recipeId, @Name, @Amount, @Unit);
                        """;

        public const string InsertIntoRecipesSteps = """
                        INSERT INTO Steps ( RecipeId, Number, Description)
                          VALUES (@recipeId, @Number, @Description);
                        """;

        public const string InsertIntoRecipesTags = """
                        INSERT INTO Tags ( RecipeId, Description)
                          VALUES (@recipeId, @Description);
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
                        JOIN Ingredients ON Recipes.Id = Ingredients.RecipeId
                        JOIN Steps ON Recipes.Id = Steps.RecipeId
                        JOIN Tags ON Recipes.Id = Tags.RecipeId
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
                        JOIN Ingredients ON Recipes.Id = Ingredients.RecipeId
                        JOIN Steps ON Recipes.Id = Steps.RecipeId
                        JOIN Tags ON Recipes.Id = Tags.RecipeId
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
                        JOIN Ingredients ON Recipes.Id = Ingredients.RecipeId
                        JOIN Steps ON Recipes.Id = Steps.RecipeId
                        JOIN Tags ON Recipes.Id = Tags.RecipeId
                        WHERE Recipes.Guid = @id
                        GROUP BY Recipes.Slug, Recipes.Title, Recipes.Author, 
                                Recipes.NumberOfPortions, Recipes.Calories
            """;

        public const string GetAllTitles = @"SELECT Recipes.Title FROM Recipes";

        public const string GetAllTitlesWithTag = @"SELECT Recipes.Title FROM Recipes 
                                                    JOIN Tags ON Recipes.Id = Tags.RecipeId
                                                    WHERE Tags.Description = @tag";

        public const string GetIdFromSlug = @"SELECT Recipes.Guid FROM Recipes
                                              WHERE Recipes.Slug = @slug";

        public const string Update = @"UPDATE Recipes SET Title = @Title, Author = @Author,
                                                 NumberOfPortions = @NumberOfPortions,
                                                 Calories = @Calories,
                                                 Slug = @Slug";

        public const string DeleteById = @"DELETE FROM Recipes WHERE Guid = @id";

        public const string DeleteBySlug = @"DELETE FROM Recipes WHERE Slug = @slug";

        public const string DeleteIngredients = @"DELETE FROM Ingredients
                                                  WHERE RecipeSlug = @Slug";

        public const string DeleteSteps = @"DELETE FROM Steps
                                                  WHERE RecipeSlug = @Slug";

        public const string DeleteTags = @"DELETE FROM Tags
                                                  WHERE RecipeSlug = @Slug";

        public const string ExistsById = @"SELECT COUNT(1) FROM Recipes WHERE Guid = @id";

        public const string ExistsBySlug = @"SELECT COUNT(1) FROM Recipes WHERE Slug = @slug";

        public const string GetLastInsertRowId = @"SELECT last_insert_rowid()";
    }
}