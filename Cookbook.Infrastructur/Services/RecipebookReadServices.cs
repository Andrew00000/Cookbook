using Cookbook.Application.Database;
using Cookbook.Application.Services;
using Cookbook.Domain.Models;

namespace Cookbook.Infrastructur.Services
{
    internal class RecipebookReadServices : IRecipebookReadServices
    {
        private readonly IRecipebookRepository recipebookRepository;

        public RecipebookReadServices(IRecipebookRepository recipebookRepository)
        {
            this.recipebookRepository = recipebookRepository;
        }

        public Task<IEnumerable<Recipe>> GetAllAsync()
            => recipebookRepository.GetAllAsync();

        public Task<IEnumerable<string>> GetAllTitlesAsync()
            => recipebookRepository.GetAllTitlesAsync();

        public Task<IEnumerable<string>> GetAllTitlesWithTagAsync(string tag)
            => recipebookRepository.GetAllTitlesWithTagAsync(tag);

        public Task<Recipe?> GetByIdAsync(Guid id)
            => recipebookRepository.GetByIdAsync(id);

        public Task<Recipe?> GetBySlugAsync(string slug)
            => recipebookRepository.GetBySlugAsync(slug);

        public Task<Guid> GetIdFromSlugAsync(string idOrSlug)
            => recipebookRepository.GetIdFromSlugAsync(idOrSlug);
    }
}
