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
        Task<IEnumerable<Recipe>> GetAllAsync();
        Task<Recipe?> GetByIdAsync(Guid id);
        Task<Recipe?> GetBySlugAsync(string slug);
        Task<IEnumerable<string>> GetAllTitlesAsync();
        Task<IEnumerable<string>> GetAllTitlesWithTagAsync(string tag);
        Task<Guid> GetIdFromSlugAsync(string idOrSlug);
    }
}
