using Cookbook.Application.Database;
using Cookbook.Domain.Models;
using Cookbook.Repository.Database;
using Dapper;
using System.Data;

namespace Cookbook.Repository.Repositories
{
    public class RecipebookRepository : IRecipebookRepository
    {
        private readonly IDbManipulator dbManipulator;

        public RecipebookRepository(IDbManipulator dbManipulator)
        {
            this.dbManipulator = dbManipulator;
        }

        public async Task<bool> CreateAsync(Recipe recipe, CancellationToken token)
        {
            return await dbManipulator.RunQuery(async connection =>
            {
                using var transaction = connection.BeginTransaction();

                var result = await connection.ExecuteAsync(
                                new CommandDefinition(SqliteCommandTexts.Create,
                                                      recipe, cancellationToken: token));

                if (result > 0)
                {
                    recipe.Id = connection.QuerySingle<long>(SqliteCommandTexts.GetLastInsertRowId);
                    await AttachIngredientsToRecipe(recipe, connection, token);
                    await AttachStepsToRecipe(recipe, connection, token);
                    await AttachTagsToRecipe(recipe, connection, token);
                }

                transaction.Commit();

                return result > 0;
            }, token);
        }

        public async Task<IEnumerable<Recipe>> GetAllAsync(CancellationToken token)
        {
            return await dbManipulator.RunQuery(async connection =>
            {
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
            }, token);
        }

        public async Task<Recipe?> GetByIdAsync(long id, CancellationToken token)
        {
            return await dbManipulator.RunQuery(async connection =>
            {

                var rawRecipe = await connection.QuerySingleOrDefaultAsync(
                            new CommandDefinition(SqliteCommandTexts.GetById, 
                                                  new { id }, cancellationToken: token));

                if (rawRecipe is null)
                {
                    return null;
                }

                return ParseRecipe(rawRecipe);
            }, token);
        }

        public async Task<Recipe?> GetBySlugAsync(string slug, CancellationToken token)
        {
            return await dbManipulator.RunQuery(async connection =>
            {

                var rawRecipe = await connection.QuerySingleOrDefaultAsync(
                            new CommandDefinition(SqliteCommandTexts.GetBySlug, 
                                                  new { slug }, cancellationToken: token));

                if (rawRecipe is null)
                {
                    return null;
                }

                return ParseRecipe(rawRecipe);
            }, token);
        }

        public async Task<IEnumerable<string>> GetAllTitlesAsync(CancellationToken token)
        {
            return await dbManipulator.RunQuery(async connection =>
            {

                var titles = await connection.QueryAsync<string>(
                                new CommandDefinition(SqliteCommandTexts.GetAllTitles,
                                                      cancellationToken: token));

                return titles?.Order() ?? Enumerable.Empty<string>();
            }, token);
        }

        public async Task<IEnumerable<string>> GetAllTitlesWithTagAsync(string tag, 
                                                             CancellationToken token)
        {
            return await dbManipulator.RunQuery(async connection =>
            {
                var titles = await connection.QueryAsync<string>(
                                new CommandDefinition(SqliteCommandTexts.GetAllTitlesWithTag, 
                                                      new { tag }, cancellationToken: token));

                if (titles is null)
                {
                    return Enumerable.Empty<string>();
                }

                return titles.Order();
            }, token);
        }

        public async Task<bool> UpdateByIdAsync(Recipe recipe, CancellationToken token)
        {
            return await dbManipulator.RunQuery(async connection =>
            {
                using var transaction = connection.BeginTransaction();

                var ingredientsDeleted = await DeleteIngredientsByIdAsync(recipe.Id, connection, token);
                var stepsDeleted = await DeleteStepsByIdAsync(recipe.Id, connection, token);
                var tagsDeleted = await DeleteTagsByIdAsync(recipe.Id, connection, token);

                if (!ingredientsDeleted || !stepsDeleted)
                {
                    return false;
                }

                var result = await UpdateAsync(recipe, connection, token);

                transaction.Commit();
                return result;
            }, token);
        }

        public async Task<bool> DeleteByIdAsync(long id, CancellationToken token)
        {
            return await dbManipulator.RunQuery(async connection =>
            {
                using var transaction = connection.BeginTransaction();

                var result = await connection.ExecuteAsync(new CommandDefinition(
                                                SqliteCommandTexts.DeleteById, 
                                                new { id },
                                                cancellationToken: token));

                transaction.Commit();
                return result > 0;
            }, token);
        }

        public async Task<bool> DeleteBySlugAsync(string slug, CancellationToken token)
        {
            return await dbManipulator.RunQuery(async connection =>
            {
                using var transaction = connection.BeginTransaction();

                var result = await connection.ExecuteAsync(new CommandDefinition(
                                                SqliteCommandTexts.DeleteBySlug, 
                                                new { slug },
                                                cancellationToken: token));

                transaction.Commit();
                return result > 0;
            }, token);
        }

        public async Task<long> GetIdFromSlugAsync(string slug, CancellationToken token)
        {
            return await dbManipulator.RunQuery(async connection =>
            {
                var rawId = await connection.QuerySingleOrDefaultAsync(
                       new CommandDefinition(SqliteCommandTexts.GetIdFromSlug,
                                             new { slug }, cancellationToken: token));
                if(rawId is null)
                {
                    throw new ArgumentNullException("There is no recipe with that slug");
                }

                return (long)rawId.Id;
            }, token);
        }

        public async Task<bool> ExistsByIdAsync(long id, CancellationToken token)
        {
            return await dbManipulator.RunQuery(async connection =>
            {
                return await connection.ExecuteScalarAsync<bool>(
                                new CommandDefinition(SqliteCommandTexts.ExistsById,
                                    new { id },
                                    cancellationToken: token));
            }, token);
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
                Id = (long)rawRecipe.Id,
                NumberOfPortions = (int)rawRecipe.NumberOfPortions,
                Calories = (int)rawRecipe.Calories,
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
                                        Unit = x.Split(' ')[1],
                                        Name = x.Split(' ')[2]
                                    });
        private async Task AttachTagsToRecipe(Recipe recipe, IDbConnection connection,
                                              CancellationToken token)
        {
            foreach (var tag in recipe.Tags)
            {
                await connection.ExecuteAsync(
                        new CommandDefinition(SqliteCommandTexts.InsertIntoRecipesTags,
                            new { recipe.Id, Description = tag },
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
                            new { recipe.Id, Number = index, Description = step },
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
                            new { recipe.Id, ingredient.Name, ingredient.Amount, ingredient.Unit },
                            cancellationToken: token));
            }
        }

        private async Task<bool> UpdateAsync(Recipe recipe, IDbConnection connection,
                                             CancellationToken token)
        {

            var result = await connection.ExecuteAsync(
                            new CommandDefinition(SqliteCommandTexts.Update, recipe,
                                                  cancellationToken: token));

            if (result > 0)
            {
                await AttachIngredientsToRecipe(recipe, connection, token);
                await AttachStepsToRecipe(recipe, connection, token);
                await AttachTagsToRecipe(recipe, connection, token);
            }

            return result > 0;
        }

        private async Task<bool> DeleteByIdAsync(long id, IDbConnection connection,
                                                 CancellationToken token)
        {
            var result = await connection.ExecuteAsync(new CommandDefinition(
                                            SqliteCommandTexts.DeleteById, new { id },
                                            cancellationToken: token));
            return result > 0;
        }

        private async Task<bool> DeleteIngredientsByIdAsync(long id, IDbConnection connection,
                                                 CancellationToken token)
        {
            var result = await connection.ExecuteAsync(new CommandDefinition(
                                            SqliteCommandTexts.DeleteIngredientsById, new { id },
                                            cancellationToken: token));
            return result > 0;
        }

        private async Task<bool> DeleteStepsByIdAsync(long id, IDbConnection connection,
                                                 CancellationToken token)
        {
            var result = await connection.ExecuteAsync(new CommandDefinition(
                                            SqliteCommandTexts.DeleteStepsById, new { id },
                                            cancellationToken: token));
            return result > 0;
        }

        private async Task<bool> DeleteTagsByIdAsync(long id, IDbConnection connection,
                                                 CancellationToken token)
        {
            var result = await connection.ExecuteAsync(new CommandDefinition(
                                            SqliteCommandTexts.DeleteTagsById, new { id },
                                            cancellationToken: token));
            return result > 0;
        }
    }
}
