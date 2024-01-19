using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace Cookbook.Application.Database
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync ();
    }

    public class SqliteConnectionFactory : IDbConnectionFactory
    {
        private readonly string connectionString = 
                    $@"Data Source={Directory.GetParent(Environment.CurrentDirectory)!.FullName}\Cookbook.Application\Cookbook.db";

        static SqliteConnectionFactory()
        {
            Batteries.Init();
        }
        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();

            return connection;
        }
    }
}
