using Cookbook.Repository.Database.Schema;

namespace Cookbook.Repository.Repositories
{
    public static class SqliteCommandTexts
    {
        public const string Create = $@"
                        INSERT INTO {DbTables.Recipes} ({RecipesTable.Title}, {RecipesTable.Author}, 
                                                        {RecipesTable.NumberOfPortions}, {RecipesTable.Calories}, 
                                                        {RecipesTable.Slug})
                          VALUES (@Title, @Author, @NumberOfPortions, @Calories, @Slug);";

        public const string InsertIntoRecipesIngredients = $@"
                        INSERT INTO {DbTables.Ingredients} ( {IngredientsTable.RecipeId}, {IngredientsTable.Name}, 
                                                             {IngredientsTable.Amount}, {IngredientsTable.Unit})
                          VALUES (@Id, @Name, @Amount, @Unit);";

        public const string InsertIntoRecipesSteps = $@"
                        INSERT INTO {DbTables.Steps} ( {StepsTable.RecipeId}, {StepsTable.Number}, 
                                                       {StepsTable.Description})
                          VALUES (@Id, @Number, @Description);";

        public const string InsertIntoRecipesTags = $@"
                        INSERT INTO {DbTables.Tags} ( {TagsTable.RecipeId}, {TagsTable.Description})
                          VALUES (@Id, @Description);";

        public const string GetAll = $@"{GetJoinedBase}
                                        {DefaultGroupBy}";

        public const string GetBySlug = $@"{GetJoinedBase}
                        WHERE {DbTables.Recipes}.{RecipesTable.Slug} = @slug
                        {DefaultGroupBy}";

        public const string GetById = $@"{GetJoinedBase}
                        WHERE {DbTables.Recipes}.{RecipesTable.Id} = @id
                        {DefaultGroupBy}";

        public const string GetAllTitles = $@"SELECT {DbTables.Recipes}.{RecipesTable.Title} FROM {DbTables.Recipes}";

        public const string GetAllTitlesWithTag = $@"SELECT {DbTables.Recipes}.{RecipesTable.Title} 
                        FROM {DbTables.Recipes} 
                        JOIN Tags ON {DbTables.Recipes}.{RecipesTable.Id} = {DbTables.Tags}.{TagsTable.RecipeId}
                        WHERE {DbTables.Tags}.{TagsTable.Description} = @tag";

        public const string GetIdFromSlug = $@"SELECT {DbTables.Recipes}.{RecipesTable.Id} FROM {DbTables.Recipes}
                                              WHERE {DbTables.Recipes}.{RecipesTable.Slug} = @slug";

        public const string Update = $@"UPDATE {DbTables.Recipes} SET {RecipesTable.Title} = @Title, 
                                                                      {RecipesTable.Author} = @Author,
                                                                      {RecipesTable.NumberOfPortions} = @NumberOfPortions,
                                                                      {RecipesTable.Calories} = @Calories,
                                                                      {RecipesTable.Slug} = @Slug
                                                                WHERE {RecipesTable.Id } = @Id";

        public const string DeleteById = $@"DELETE FROM {DbTables.Recipes} WHERE {RecipesTable.Id} = @id";

        public const string DeleteBySlug = $@"DELETE FROM {DbTables.Recipes} WHERE {RecipesTable.Slug} = @slug";

        public const string DeleteIngredientsById = $@"DELETE FROM {DbTables.Ingredients} 
                                                              WHERE {IngredientsTable.RecipeId} = @id";

        public const string DeleteStepsById = $@"DELETE FROM {DbTables.Steps} 
                                                              WHERE {StepsTable.RecipeId} = @id";

        public const string DeleteTagsById = $@"DELETE FROM {DbTables.Tags} 
                                                              WHERE {TagsTable.RecipeId} = @id";

        public const string ExistsById = $@"SELECT COUNT(1) FROM {DbTables.Recipes} WHERE {RecipesTable.Id} = @id";

        public const string ExistsBySlug = $@"SELECT COUNT(1) FROM {DbTables.Recipes} WHERE {RecipesTable.Slug} = @slug";

        public const string GetLastInsertRowId = @"SELECT last_insert_rowid()";

        private const string GetJoinedBase = $@"
                        SELECT {DbTables.Recipes}.{RecipesTable.Title}, 
                               {DbTables.Recipes}.{RecipesTable.Author}, 
                               {DbTables.Recipes}.{RecipesTable.NumberOfPortions}, 
                               {DbTables.Recipes}.{RecipesTable.Calories}, 
                               {DbTables.Recipes}.{RecipesTable.Slug}, 
                               {DbTables.Recipes}.{RecipesTable.Id},
                               GROUP_CONCAT({DbTables.Ingredients}.{IngredientsTable.Amount} || ' ' || 
                                            {DbTables.Ingredients}.{IngredientsTable.Unit} || ' ' || 
                                            {DbTables.Ingredients}.{IngredientsTable.Name}, ', ') AS IngredientsList,
                               GROUP_CONCAT({DbTables.Steps}.{StepsTable.Number} || '. ' || 
                                            {DbTables.Steps}.{StepsTable.Description}, ', ') AS StepsList,
                               GROUP_CONCAT({DbTables.Tags}.{TagsTable.Description}, ', ') AS TagsList
                        FROM {DbTables.Recipes}
                        JOIN {DbTables.Ingredients} ON {DbTables.Recipes}.{RecipesTable.Id} = {DbTables.Ingredients}.{IngredientsTable.RecipeId}
                        JOIN {DbTables.Steps} ON {DbTables.Recipes}.{RecipesTable.Id} = {DbTables.Steps}.{StepsTable.RecipeId}
                        JOIN {DbTables.Tags} ON {DbTables.Recipes}.{RecipesTable.Id} = {DbTables.Tags}.{TagsTable.RecipeId}";

        private const string DefaultGroupBy = 
                     $@"GROUP BY {DbTables.Recipes}.{RecipesTable.Slug}, {DbTables.Recipes}.{RecipesTable.Title}, 
                                 {DbTables.Recipes}.{RecipesTable.Author}, {DbTables.Recipes}.{RecipesTable.NumberOfPortions},
                                 {DbTables.Recipes}.{RecipesTable.Calories}";
    }
}