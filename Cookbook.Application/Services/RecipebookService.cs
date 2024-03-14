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

        public Task<IEnumerable<Recipe>> GetAllAsync(CancellationToken token)
            => recipebookRepository.GetAllAsync(token);

        public Task<IEnumerable<string>> GetAllTitlesAsync(CancellationToken token)
            => recipebookRepository.GetAllTitlesAsync(token);

        public Task<IEnumerable<string>> GetAllTitlesWithTagAsync(string tag,
                                                       CancellationToken token)
            => recipebookRepository.GetAllTitlesWithTagAsync(tag, token);

        public Task<Recipe?> GetByIdAsync(long id, CancellationToken token)
            => recipebookRepository.GetByIdAsync(id, token);

        public Task<Recipe?> GetBySlugAsync(string slug, CancellationToken token)
            => recipebookRepository.GetBySlugAsync(slug, token);

        public Task<long> GetIdFromSlugAsync(string idOrSlug, CancellationToken token)
            => recipebookRepository.GetIdFromSlugAsync(idOrSlug, token);


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

        public Task<bool> DeleteByIdAsync(long id, CancellationToken token)
            => recipebookRepository.DeleteByIdAsync(id, token);

        public Task<bool> DeleteBySlugAsync(string slug, CancellationToken token)
            => recipebookRepository.DeleteBySlugAsync(slug, token);
    }
}
