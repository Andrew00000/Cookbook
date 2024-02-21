using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace Cookbook.Repository.Database
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync(CancellationToken token);
    }
}
