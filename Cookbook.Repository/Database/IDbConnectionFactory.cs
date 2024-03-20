using System.Data;

namespace Cookbook.Repository.Database
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync(CancellationToken token);
    }
}
