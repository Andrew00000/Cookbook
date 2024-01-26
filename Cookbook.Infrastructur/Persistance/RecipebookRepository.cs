using Cookbook.Application.Database;
using Cookbook.Domain.Models;
using Dapper;
using System.Data;

namespace Cookbook.Infrastructur.Persistance
{
    public class RecipebookRepository : IRecipebookRepository
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public RecipebookRepository(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<bool> CreateAsync(Recipe recipe, CancellationToken token)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(
                            new CommandDefinition(SqliteCommandTexts.Create,
                                                  recipe, cancellationToken: token));

            if (result > 0)
            {
                await AttachIngredientsToRecipe(recipe, connection, token);
                await AttachStepsToRecipe(recipe, connection, token);
                await AttachTagsToRecipe(recipe, connection, token);
            }

            transaction.Commit();

            return result > 0;
        }

        public async Task<IEnumerable<Recipe>> GetAllAsync(CancellationToken token)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
            var rawRecipes = await connection.QueryAsync(
                            new CommandDefinition(SqliteCommandTexts.GetAll,
                                                  cancellationToken: token));

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

        public async Task<Recipe?> GetByIdAsync(Guid id, CancellationToken token)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync(token);

            var rawRecipe = await connection.QuerySingleOrDefaultAsync(
                            new CommandDefinition(SqliteCommandTexts.GetById, 
                                                  new { id }, cancellationToken: token));

            if (rawRecipe is null)
            {
                return null;
            }

            return ParseRecipe(rawRecipe);
        }

        public async Task<Recipe?> GetBySlugAsync(string slug, CancellationToken token)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync(token);

            var rawRecipe = await connection.QuerySingleOrDefaultAsync(
                            new CommandDefinition(SqliteCommandTexts.GetBySlug, 
                                                  new { slug }, cancellationToken: token));

            if (rawRecipe is null)
            {
                return null;
            }

            return ParseRecipe(rawRecipe);
        }

        public async Task<IEnumerable<string>> GetAllTitlesAsync(CancellationToken token)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync(token);

            var titles = await connection.QueryAsync<string>(
                                new CommandDefinition(SqliteCommandTexts.GetAllTitles,
                                                      cancellationToken: token));

            if (titles is null)
            {
                return Enumerable.Empty<string>();
            }

            return titles.Order();
        }

        public async Task<IEnumerable<string>> GetAllTitlesWithTagAsync(string tag, 
                                                             CancellationToken token)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync(token);

            var titles = await connection.QueryAsync<string>(
                                new CommandDefinition(SqliteCommandTexts.GetAllTitlesWithTag, 
                                                      new { tag }, cancellationToken: token));

            if (titles is null)
            {
                return Enumerable.Empty<string>();
            }

            return titles.Order();
        }

        public async Task<bool> UpdateByIdAsync(Recipe recipe, CancellationToken token) 
        //not the best approach TODO: make it better :D
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var deleted = await DeleteByIdAsync(recipe.Id, connection, token);
            if (!deleted)
            {
                return false;
            }
            var result = await CreateAsync(recipe, connection, token);

            transaction.Commit();
            return result;
        }

        public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(
                                            SqliteCommandTexts.DeleteById, 
                                            new { id },
                                            cancellationToken: token));

            transaction.Commit();
            return result > 0;
        }

        public async Task<bool> DeleteBySlugAsync(string slug, CancellationToken token)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
            using var transaction = connection.BeginTransaction();

            var result = await connection.ExecuteAsync(new CommandDefinition(
                                            SqliteCommandTexts.DeleteBySlug, 
                                            new { slug },
                                            cancellationToken: token));

            transaction.Commit();
            return result > 0;
        }

        public async Task<Guid> GetIdFromSlugAsync(string slug, CancellationToken token)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
            var rawGuid = await connection.QuerySingleOrDefaultAsync(
                       new CommandDefinition(SqliteCommandTexts.GetIdFromSlug,
                                             new { slug }, cancellationToken: token));
            return new Guid(rawGuid!.Guid);
        }

        public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
            return await connection.ExecuteScalarAsync<bool>(
                                new CommandDefinition(SqliteCommandTexts.ExistsById, 
                                    new { id },
                                    cancellationToken: token));
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
        private async Task AttachTagsToRecipe(Recipe recipe, IDbConnection connection,
                                              CancellationToken token)
        {
            foreach (var tag in recipe.Tags)
            {
                await connection.ExecuteAsync(
                        new CommandDefinition(SqliteCommandTexts.InsertIntoRecipesTags,
                            new { recipe.Slug, Description = tag },
                            cancellationToken: token));
            }
        }

        private async Task AttachStepsToRecipe(Recipe recipe, IDbConnection connection,
                                               CancellationToken token)
        {
            var index = 1;
            foreach (var step in recipe.Steps)
            {
                await connection.ExecuteAsync(
                        new CommandDefinition(SqliteCommandTexts.InsertIntoRecipesSteps,
                            new { recipe.Slug, Number = index, Description = step },
                            cancellationToken: token));
                index++;
            }
        }

        private async Task AttachIngredientsToRecipe(Recipe recipe, IDbConnection connection,
                                                     CancellationToken token)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                await connection.ExecuteAsync(
                        new CommandDefinition(SqliteCommandTexts.InsertIntoRecipesIngredients,
                            new { recipe.Slug, ingredient.Name, ingredient.Amount, ingredient.Unit },
                            cancellationToken: token));
            }
        }

        private async Task<bool> CreateAsync(Recipe recipe, IDbConnection connection,
                                             CancellationToken token)
        {
            var result = await connection.ExecuteAsync(
                            new CommandDefinition(SqliteCommandTexts.Create, recipe,
                                                  cancellationToken: token));

            if (result > 0)
            {
                await AttachIngredientsToRecipe(recipe, connection, token);
                await AttachStepsToRecipe(recipe, connection, token);
                await AttachTagsToRecipe(recipe, connection, token);
            }

            return result > 0;
        }

        private async Task<bool> DeleteByIdAsync(Guid id, IDbConnection connection,
                                                 CancellationToken token)
        {
            var result = await connection.ExecuteAsync(new CommandDefinition(
                                            SqliteCommandTexts.DeleteById, new { id },
                                            cancellationToken: token));
            return result > 0;
        }
    }
}
