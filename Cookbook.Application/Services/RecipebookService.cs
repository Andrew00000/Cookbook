using Cookbook.Application.Database;
using Cookbook.Domain.Models;
using FluentValidation;

namespace Cookbook.Application.Services
{
    internal class RecipebookService : IRecipebookReadService, IRecipebookWriteService
    {
        private readonly IRecipebookRepository recipebookRepository;
        private readonly IValidator<Recipe> recipeValidator;

        public RecipebookService(IRecipebookRepository recipebookRepository,
                                       IValidator<Recipe> recipeValidator)
        {
            this.recipebookRepository = recipebookRepository;
            this.recipeValidator = recipeValidator;
        }

        public async Task<IEnumerable<Recipe>> GetAllAsync(CancellationToken token)
            => await recipebookRepository.GetAllAsync(token);

        public async Task<IEnumerable<string>> GetAllTitlesAsync(CancellationToken token)
            => await recipebookRepository.GetAllTitlesAsync(token);

        public async Task<IEnumerable<string>> GetAllTitlesWithTagAsync(string tag,
                                                       CancellationToken token)
            => await recipebookRepository.GetAllTitlesWithTagAsync(tag, token);

        public async Task<Recipe?> GetByIdAsync(long id, CancellationToken token)
            => await recipebookRepository.GetByIdAsync(id, token);

        public async Task<Recipe?> GetBySlugAsync(string slug, CancellationToken token)
            => await recipebookRepository.GetBySlugAsync(slug, token);

        public async Task<long> GetIdFromSlugAsync(string idOrSlug, CancellationToken token)
            => await recipebookRepository.GetIdFromSlugAsync(idOrSlug, token);


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

        public async Task<bool> DeleteByIdAsync(long id, CancellationToken token)
            => await recipebookRepository.DeleteByIdAsync(id, token);

        public async Task<bool> DeleteBySlugAsync(string slug, CancellationToken token)
            => await recipebookRepository.DeleteBySlugAsync(slug, token);
    }
}
