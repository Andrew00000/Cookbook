using Cookbook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Application.Services
{
    public interface IRecipebookReadServices
    {
        Task<IEnumerable<Recipe>> GetAllAsync(CancellationToken token = default);
        Task<Recipe?> GetByIdAsync(Guid id, CancellationToken token = default);
        Task<Recipe?> GetBySlugAsync(string slug, CancellationToken token = default);
        Task<IEnumerable<string>> GetAllTitlesAsync(CancellationToken token = default);
        Task<IEnumerable<string>> GetAllTitlesWithTagAsync(string tag, 
                                                    CancellationToken token = default);
        Task<Guid> GetIdFromSlugAsync(string idOrSlug, CancellationToken token = default);
    }
}
