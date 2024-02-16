using Cookbook.Application.Database;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Data;

namespace Cookbook.Infrastructur.Persistance
{
    public class SqliteConnectionFactory : IDbConnectionFactory
    {
        private readonly string connectionString =
                    $@"Data Source={Directory.GetParent(Environment.CurrentDirectory)!.FullName}\Cookbook.Application\Cookbook.db";

        static SqliteConnectionFactory()
        {
            Batteries.Init();
        }
        public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token)
        {
            var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync(token);

            return connection;
        }
    }
}
