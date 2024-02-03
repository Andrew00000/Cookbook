using Cookbook.Application.Database;
using Cookbook.Domain.Models;
using FluentValidation;

namespace Cookbook.Application.Services
{
    internal class RecipebookWriteServices : IRecipebookWriteServices
    {
        private readonly IRecipebookRepository recipebookRepository;
        private readonly IValidator<Recipe> recipeValidator;

        public RecipebookWriteServices(IRecipebookRepository recipebookRepository,
                                       IValidator<Recipe> recipeValidator)
        {
            this.recipebookRepository = recipebookRepository;
            this.recipeValidator = recipeValidator;
        }

        public async Task<bool> CreateAsync(Recipe recipe, CancellationToken token = default)
        {
            await recipeValidator.ValidateAndThrowAsync(recipe, cancellationToken: token);
            return await recipebookRepository.CreateAsync(recipe, token);
        }

        public async Task<Recipe?> UpdateByIdAsync(Recipe recipe,
                                                   CancellationToken token = default)
        {
            await recipeValidator.ValidateAndThrowAsync(recipe, cancellationToken: token);

            var doesRecipeExists = await recipebookRepository.ExistsByIdAsync(recipe.Id, token);
            if (!doesRecipeExists)
            {
                return null;
            }

            await recipebookRepository.UpdateByIdAsync(recipe, token);
            return recipe;
        }

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
            => recipebookRepository.DeleteByIdAsync(id, token);

        public Task<bool> DeleteBySlugAsync(string slug, CancellationToken token = default)
            => recipebookRepository.DeleteBySlugAsync(slug, token);
    }
}
