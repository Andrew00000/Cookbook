using Cookbook.Application.Database;
using Cookbook.Domain.Models;

namespace Cookbook.Application.Services
{
    internal class RecipebookReadService : IRecipebookReadService
    {
        private readonly IRecipebookRepository recipebookRepository;

        public RecipebookReadService(IRecipebookRepository recipebookRepository)
        {
            this.recipebookRepository = recipebookRepository;
        }

        public Task<IEnumerable<Recipe>> GetAllAsync(CancellationToken token = default)
            => recipebookRepository.GetAllAsync(token);

        public Task<IEnumerable<string>> GetAllTitlesAsync(CancellationToken token = default)
            => recipebookRepository.GetAllTitlesAsync(token);

        public Task<IEnumerable<string>> GetAllTitlesWithTagAsync(string tag,
                                                       CancellationToken token = default)
            => recipebookRepository.GetAllTitlesWithTagAsync(tag, token);

        public Task<Recipe?> GetByIdAsync(Guid id, CancellationToken token = default)
            => recipebookRepository.GetByIdAsync(id, token);

        public Task<Recipe?> GetBySlugAsync(string slug, CancellationToken token = default)
            => recipebookRepository.GetBySlugAsync(slug, token);

        public Task<Guid> GetIdFromSlugAsync(string idOrSlug, CancellationToken token = default)
            => recipebookRepository.GetIdFromSlugAsync(idOrSlug, token);
    }
}
