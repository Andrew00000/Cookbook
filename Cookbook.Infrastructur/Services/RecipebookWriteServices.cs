using Cookbook.Application.Database;
using Cookbook.Application.Services;
using Cookbook.Domain.Models;

namespace Cookbook.Infrastructur.Services
{
    internal class RecipebookWriteServices : IRecipebookWriteServices
    {
        private readonly IRecipebookRepository recipebookRepository;

        public RecipebookWriteServices(IRecipebookRepository recipebookRepository)
        {
            this.recipebookRepository = recipebookRepository;
        }

        public Task<bool> CreateAsync(Recipe recipe)
            => recipebookRepository.CreateAsync(recipe);

        public async Task<Recipe?> UpdateByIdAsync(Recipe recipe)
        {
            var doesRecipeExists = await recipebookRepository.ExistsByIdAsync(recipe.Id);
            if (!doesRecipeExists)
            {
                return null;
            }

            await recipebookRepository.UpdateByIdAsync(recipe);
            return recipe;
        }

        public Task<bool> DeleteByIdAsync(Guid id)
            => recipebookRepository.DeleteByIdAsync(id);

        public Task<bool> DeleteBySlugAsync(string slug)
            => recipebookRepository.DeleteBySlugAsync(slug);
    }
}
