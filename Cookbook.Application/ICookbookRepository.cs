using Cookbook.Domain.Models;

namespace Cookbook.Application
{
    public interface ICookbookRepository
    {
        Task<bool> CreateAsync(Recipe recipe);
        Task<IEnumerable<Recipe>> GetAllAsync();
        Task<bool> ExistsByIdAsync(Guid id);
        Task<Recipe?> GetByIdAsync(Guid id);
        Task<Recipe?> GetBySlugAsync(string slug);
        Task<IEnumerable<string>> GetAllTitlesAsync();
        Task<IEnumerable<string>> GetAllTitlesWithTagAsync(string tag);
        Task<bool> UpdateByIdAsync(Recipe recipe);
        Task<bool> DeleteByIdAsync(Guid id);
        Task<bool> DeleteBySlugAsync(string slug);
        Task<Guid> GetIdFromSlugAsync(string idOrSlug);
    }
}
