using Cookbook.Application.Database;
using Cookbook.Application.Services;
using Cookbook.Domain.Models;
using FluentValidation;

namespace Cookbook.Infrastructur.Services
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

        public async Task<bool> CreateAsync(Recipe recipe)
        {
            await recipeValidator.ValidateAndThrowAsync(recipe);
            return await recipebookRepository.CreateAsync(recipe);
        }

        public async Task<Recipe?> UpdateByIdAsync(Recipe recipe)
        {
            await recipeValidator.ValidateAndThrowAsync(recipe);

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
