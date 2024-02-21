using Microsoft.Data.Sqlite;
using System.Data;

namespace Cookbook.Repository.Database
{
    public class SqliteConnectionFactory : IDbConnectionFactory
    {
        private readonly string connectionString =
                    $@"Data Source={Directory.GetParent(Environment.CurrentDirectory)!.FullName}\Cookbook.Application\Cookbook.db";

        public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token)
        {
            var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync(token);

            return connection;
        }
    }
}
