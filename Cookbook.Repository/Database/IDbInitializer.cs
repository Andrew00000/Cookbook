using Cookbook.Application.Utilities;

namespace Cookbook.Repository.Database
{
    public interface IDbInitializer : IInitializable
    {
        Task Initialize();
    }
}