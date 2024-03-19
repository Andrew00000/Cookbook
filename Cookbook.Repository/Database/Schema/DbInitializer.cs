using Cookbook.Repository.Repositories;
using Dapper;

namespace Cookbook.Repository.Database.Schema
{
    internal class DbInitializer : IDbInitializer
    {
        private readonly string dbFilePath;
        private readonly IDbManipulator dbManipulator;

        public DbInitializer(DatabaseConfiguration databaseConfiguration, IDbManipulator dbManipulator)
        {
            dbFilePath = databaseConfiguration.Path!;
            this.dbManipulator = dbManipulator;
        }

        public async Task Initialize()
        {
            if (!File.Exists(dbFilePath))
            {
                using (File.Create(dbFilePath));
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
                {
                    await dbManipulator.RunQuery(async connection =>
                    {
                        await connection.ExecuteAsync(new CommandDefinition(SqliteCommandTexts.CreateRecipesTable));

                        await connection.ExecuteAsync(new CommandDefinition(SqliteCommandTexts.CreateIngredientsTable));

                        await connection.ExecuteAsync(new CommandDefinition(SqliteCommandTexts.CreateStepsTable));

                        await connection.ExecuteAsync(new CommandDefinition(SqliteCommandTexts.CreateTagsTable));

                        await connection.ExecuteAsync(new CommandDefinition(SqliteCommandTexts.CreateSlugIndex));
                    }, cts.Token);
                }
            }
        }
    }
}
