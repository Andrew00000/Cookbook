using Cookbook.Domain.Models;

namespace Cookbook.Application.Services
{
    public interface IRecipebookWriteServices
    {
        Task<bool> CreateAsync(Recipe recipe, CancellationToken token = default);
        Task<Recipe?> UpdateByIdAsync(Recipe recipe, CancellationToken token = default);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
        Task<bool> DeleteBySlugAsync(string slug, CancellationToken token = default);
    }
}
