using Cookbook.Domain.Models;

namespace Cookbook.Application.Database
{
    public interface IRecipebookRepository
    {
        Task<bool> CreateAsync(Recipe recipe, CancellationToken token);
        Task<IEnumerable<Recipe>> GetAllAsync(CancellationToken token);
        Task<bool> ExistsByIdAsync(long id, CancellationToken token);
        Task<Recipe?> GetByIdAsync(long id, CancellationToken token);
        Task<Recipe?> GetBySlugAsync(string slug, CancellationToken token);
        Task<IEnumerable<string>> GetAllTitlesAsync(CancellationToken token);
        Task<IEnumerable<string>> GetAllTitlesWithTagAsync(string tag, CancellationToken token);
        Task<bool> UpdateByIdAsync(Recipe recipe, CancellationToken token);
        Task<bool> DeleteByIdAsync(long id, CancellationToken token);
        Task<bool> DeleteBySlugAsync(string slug, CancellationToken token);
        Task<long> GetIdFromSlugAsync(string idOrSlug, CancellationToken token);
    }
}
