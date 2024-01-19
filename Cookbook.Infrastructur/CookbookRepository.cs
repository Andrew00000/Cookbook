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

        public Task<IEnumerable<Recipe>> GetAllAsync()
        {
            throw new NotImplementedException();
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
