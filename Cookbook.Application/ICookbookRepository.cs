using Cookbook.Domain.Models;

namespace Cookbook.Application
{
    public interface ICookbookRepository
    {
        Task<bool> CreateAsync(Recipe recipe);
        Task<Recipe?> GetByIdAsync(Guid id);
        Task<Recipe?> GetBySlugAsync(string slug);
        Task<IEnumerable<Recipe>> GetAllAsync();
        Task<bool> UpdateByIdAsync(Recipe recipe);
        Task<bool> DeleteByIdAsync(Guid id);
    }
}
