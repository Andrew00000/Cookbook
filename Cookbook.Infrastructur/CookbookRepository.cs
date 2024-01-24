using Cookbook.Application;
using Cookbook.Application.Database;
using Cookbook.Domain.Models;
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
                            new CommandDefinition(SqliteCommandTexts.InsertIntoRecipesTable, recipe));

            if (result > 0)
            {
                foreach (var ingredient in recipe.Ingredients)
                {
                    await connection.ExecuteAsync(
                            new CommandDefinition(SqliteCommandTexts.InsertIntoRecipesIngredients,
                            new { recipe.Slug, ingredient.Name, ingredient.Amount, ingredient.Unit }));
                }

                var index = 1;
                foreach (var step in recipe.Steps)
                {
                    await connection.ExecuteAsync(
                            new CommandDefinition(SqliteCommandTexts.InsertIntoRecipesSteps,
                            new { recipe.Slug, Number = index, Description = step }));
                    index++;
                }

                foreach (var tag in recipe.Tags)
                {
                    await connection.ExecuteAsync(
                            new CommandDefinition(SqliteCommandTexts.InsertIntoRecipesTags,
                            new { recipe.Slug, Description = tag }));
                }
            }

            transaction.Commit();

            return result > 0;
        }

        public async Task<IEnumerable<Recipe>> GetAllAsync()
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync();
            var rawRecipes = await connection.QueryAsync(
                            new CommandDefinition(SqliteCommandTexts.GetAllRecipes)); 

            if (rawRecipes is null)
            {
                return Enumerable.Empty<Recipe>();
            }

            var recipes = new List<Recipe>();

            foreach (var rawRecipe in rawRecipes)
            {
                var ingredients = ((string)rawRecipe.IngredientsList).Split(", ").Distinct()
                                        .Select(x => new Ingredient { Amount = int.Parse(x.Split(' ')[0]), 
                                                                      Unit = (UnitType)int.Parse(x.Split(' ')[1]), 
                                                                      Name = x.Split(' ')[2]});
                var steps = ((string)rawRecipe.StepsList).Split(", ").Distinct()
                                        .Order().Select(x => x.Split(". ")[1]);
                var tags = ((string)rawRecipe.TagsList).Split(", ").Distinct();

                recipes.Add(new Recipe
                {
                    Title = rawRecipe.Title,
                    Author = rawRecipe.Author,
                    NumberOfPortions = (int)rawRecipe.NumberOfPortions,
                    Calories = (int)rawRecipe.Calories,
                    Id = new Guid(rawRecipe.Guid),
                    Ingredients = ingredients,
                    Steps = steps,
                    Tags = tags
                });
            }

            return recipes;
        }

        public Task<bool> ExistsByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsBySlugAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Recipe?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Recipe?> GetBySlugAsync(string slug)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateByIdAsync(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateBySlugAsync(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteBySlugAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
