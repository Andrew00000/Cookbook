using Cookbook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Application.Services
{
    public interface IRecipebookReadService
    {
        Task<IEnumerable<Recipe>> GetAllAsync(CancellationToken token);
        Task<Recipe?> GetByIdAsync(Guid id, CancellationToken token);
        Task<Recipe?> GetBySlugAsync(string slug, CancellationToken token);
        Task<IEnumerable<string>> GetAllTitlesAsync(CancellationToken token);
        Task<IEnumerable<string>> GetAllTitlesWithTagAsync(string tag, 
                                                    CancellationToken token);
        Task<Guid> GetIdFromSlugAsync(string idOrSlug, CancellationToken token);
    }
}
