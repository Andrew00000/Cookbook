using Cookbook.Domain.Models;

namespace Cookbook.Application.Services
{
    public interface IRecipebookWriteService
    {
        Task<bool> CreateAsync(Recipe recipe, CancellationToken token);
        Task<Recipe?> UpdateByIdAsync(Recipe recipe, CancellationToken token);
        Task<bool> DeleteByIdAsync(Guid id, CancellationToken token);
        Task<bool> DeleteBySlugAsync(string slug, CancellationToken token);
    }
}
