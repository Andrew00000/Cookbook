using Microsoft.Data.Sqlite;
using System.Data;

namespace Cookbook.Repository.Database
{
    internal class SqliteConnectionFactory : IDbConnectionFactory
    {
        private readonly DatabaseConfiguration databaseConfiguration;

        public SqliteConnectionFactory(DatabaseConfiguration databaseConfiguration)
        {
            this.databaseConfiguration = databaseConfiguration;
        }

        public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token)
        {
            var connection = new SqliteConnection($"Data Source = {databaseConfiguration.Path}");
            await connection.OpenAsync(token);

            return connection;
        }
    }
}
