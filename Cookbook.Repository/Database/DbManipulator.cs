using System.Data;

namespace Cookbook.Repository.Database
{
    public class DbManipulator : IDbManipulator
    {
        private readonly IDbConnectionFactory dbConnectionFactory;

        public DbManipulator(IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<T> RunQuery<T>(Func<IDbConnection, Task<T>> callback, CancellationToken token)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
            return await callback(connection);
        }

        public async Task RunQuery(Func<IDbConnection, Task> callback, CancellationToken token)
        {
            using var connection = await dbConnectionFactory.CreateConnectionAsync(token);
            await callback(connection);
        }
    }
}
