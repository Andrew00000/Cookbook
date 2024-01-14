using Cookbook.Application;
using Cookbook.Domain.Models;

namespace Cookbook.Infrastructur
{
    public class CookbookRepository : ICookbookRepository
    {
        private readonly List<Recipe> recipes = new();
        public Task<bool> CreateAsync(Recipe recipe)
        {
            recipes.Add(recipe);
            return Task.FromResult(true);
        }

        public Task<Recipe?> GetByIdAsync(Guid id)
        {
            var recipe = recipes.SingleOrDefault(x => x.Id == id);
            return Task.FromResult(recipe);
        }

        public Task<Recipe?> GetBySlugAsync(string slug)
        {
            var recipe = recipes.SingleOrDefault(x => x.Slug == slug);
            return Task.FromResult(recipe);
        }

        public Task<IEnumerable<Recipe>> GetAllAsync()
        {
            return Task.FromResult(recipes.AsEnumerable());
        }

        public Task<bool> UpdateByIdAsync(Recipe recipe)
        {
            var recipeIndex = recipes.FindIndex(x => x.Id == recipe.Id);

            if (recipeIndex == -1)
            {
                return Task.FromResult(false);
            }

            recipes[recipeIndex] = recipe;

            return Task.FromResult(true);
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            var removedCount = recipes.RemoveAll(x => x.Id == id);

            var recipeRemoved = removedCount > 0;

            return Task.FromResult(recipeRemoved);
        }
    }
}
