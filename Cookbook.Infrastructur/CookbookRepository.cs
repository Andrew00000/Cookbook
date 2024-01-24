using Cookbook.Application;
using Cookbook.Application.Database;
using Cookbook.Domain.Models;
using Dapper;

namespace Cookbook.Infrastructur
{
    public class CookbookRepository : ICookbookRepository
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public CookbookRepository(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<bool> CreateAsync(Recipe recipe)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(
                            new CommandDefinition(SqliteCommandTexts.Create, recipe));

            if (result > 0)
            {
                await AttachIngredientsToRecipe(recipe, connection);
                await AttachStepsToRecipe(recipe, connection);
                await AttachTagsToRecipe(recipe, connection);
            }

            transaction.Commit();

            return result > 0;
        }

        public async Task<IEnumerable<Recipe>> GetAllAsync()
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync();
            var rawRecipes = await connection.QueryAsync(
                            new CommandDefinition(SqliteCommandTexts.GetAll));

            if (rawRecipes is null)
            {
                return Enumerable.Empty<Recipe>();
            }

            var recipes = new List<Recipe>();

            foreach (var rawRecipe in rawRecipes)
            {
                var recipe = ParseRecipe(rawRecipe);

                recipes.Add(recipe);
            }

            return recipes.OrderBy(x => x.Title);
        }

        public async Task<Recipe?> GetByIdAsync(Guid id)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync();

            var rawRecipe = await connection.QuerySingleOrDefaultAsync(
                            new CommandDefinition(SqliteCommandTexts.GetById, new { id }));

            if (rawRecipe is null)
            {
                return null;
            }

            return ParseRecipe(rawRecipe);
        }

        public async Task<Recipe?> GetBySlugAsync(string slug)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync();

            var rawRecipe = await connection.QuerySingleOrDefaultAsync(
                            new CommandDefinition(SqliteCommandTexts.GetBySlug, new { slug }));

            if (rawRecipe is null)
            {
                return null;
            }

            return ParseRecipe(rawRecipe);
        }

        public async Task<IEnumerable<string>> GetAllTitlesAsync()
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync();

            var titles = await connection.QueryAsync<string>(
                                new CommandDefinition(SqliteCommandTexts.GetAllTitles));

            if (titles is null)
            {
                return Enumerable.Empty<string>();
            }

            return titles.Order();
        }

        public async Task<IEnumerable<string>> GetAllTitlesWithTagAsync(string tag)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync();

            var titles = await connection.QueryAsync<string>(
                                new CommandDefinition(SqliteCommandTexts.GetAllTitlesWithTag, new { tag }));

            if (titles is null)
            {
                return Enumerable.Empty<string>();
            }

            return titles.Order();
        }

        public Task<bool> UpdateByIdAsync(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateBySlugAsync(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(
                                            SqliteCommandTexts.DeleteById, new { id }));

            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> DeleteBySlugAsync(string slug)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync();
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(
                                            SqliteCommandTexts.DeleteBySlug, new { slug }));

            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync();
            return await connection.ExecuteScalarAsync<bool>(
                                new CommandDefinition(SqliteCommandTexts.ExistsById, new { id }));
        }

        public async Task<bool> ExistsBySlugAsync(string slug)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync();
            return await connection.ExecuteScalarAsync<bool>(
                                new CommandDefinition(SqliteCommandTexts.ExistsBySlug, new { slug }));
        }

        private Recipe ParseRecipe(dynamic rawRecipe)
        {
            var ingredients = ParseIngredient(rawRecipe);
            var steps = ((string)rawRecipe.StepsList).Split(", ").Distinct()
                                    .Order().Select(x => x.Split(". ")[1]);
            var tags = ((string)rawRecipe.TagsList).Split(", ").Distinct();

            var recipe = new Recipe
            {
                Title = rawRecipe.Title,
                Author = rawRecipe.Author,
                NumberOfPortions = (int)rawRecipe.NumberOfPortions,
                Calories = (int)rawRecipe.Calories,
                Id = new Guid(rawRecipe.Guid),
                Ingredients = ingredients,
                Steps = steps,
                Tags = tags
            };
            return recipe;
        }

        private IEnumerable<Ingredient> ParseIngredient(dynamic rawRecipe)
            => ((string)rawRecipe.IngredientsList).Split(", ").Distinct()
                                    .Select(x => new Ingredient
                                    {
                                        Amount = int.Parse(x.Split(' ')[0]),
                                        Unit = (UnitType)int.Parse(x.Split(' ')[1]),
                                        Name = x.Split(' ')[2]
                                    });
        private async Task AttachTagsToRecipe(Recipe recipe, System.Data.IDbConnection connection)
        {
            foreach (var tag in recipe.Tags)
            {
                await connection.ExecuteAsync(
                        new CommandDefinition(SqliteCommandTexts.InsertIntoRecipesTags,
                        new { recipe.Slug, Description = tag }));
            }
        }

        private async Task AttachStepsToRecipe(Recipe recipe, System.Data.IDbConnection connection)
        {
            var index = 1;
            foreach (var step in recipe.Steps)
            {
                await connection.ExecuteAsync(
                        new CommandDefinition(SqliteCommandTexts.InsertIntoRecipesSteps,
                        new { recipe.Slug, Number = index, Description = step }));
                index++;
            }
        }

        private async Task AttachIngredientsToRecipe(Recipe recipe, System.Data.IDbConnection connection)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                await connection.ExecuteAsync(
                        new CommandDefinition(SqliteCommandTexts.InsertIntoRecipesIngredients,
                        new { recipe.Slug, ingredient.Name, ingredient.Amount, ingredient.Unit }));
            }
        }

    }
}
