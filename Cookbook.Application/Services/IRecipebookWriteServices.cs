using Cookbook.Domain.Models;

namespace Cookbook.Application.Services
{
    public interface IRecipebookWriteServices
    {
        Task<bool> CreateAsync(Recipe recipe);
        Task<Recipe?> UpdateByIdAsync(Recipe recipe);
        Task<bool> DeleteByIdAsync(Guid id);
        Task<bool> DeleteBySlugAsync(string slug);
    }
}
