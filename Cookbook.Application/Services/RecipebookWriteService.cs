using Cookbook.Application.Database;
using Cookbook.Domain.Models;
using FluentValidation;

namespace Cookbook.Application.Services
{
    internal class RecipebookWriteService : IRecipebookWriteService
    {
        private readonly IRecipebookRepository recipebookRepository;
        private readonly IValidator<Recipe> recipeValidator;

        public RecipebookWriteService(IRecipebookRepository recipebookRepository,
                                       IValidator<Recipe> recipeValidator)
        {
            this.recipebookRepository = recipebookRepository;
            this.recipeValidator = recipeValidator;
        }

        public async Task<bool> CreateAsync(Recipe recipe, CancellationToken token)
        {
            await recipeValidator.ValidateAndThrowAsync(recipe, cancellationToken: token);
            return await recipebookRepository.CreateAsync(recipe, token);
        }

        public async Task<Recipe?> UpdateByIdAsync(Recipe recipe,
                                                   CancellationToken token)
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

        public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token)
            => recipebookRepository.DeleteByIdAsync(id, token);

        public Task<bool> DeleteBySlugAsync(string slug, CancellationToken token)
            => recipebookRepository.DeleteBySlugAsync(slug, token);
    }
}
