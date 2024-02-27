using Cookbook.Application.Database;
using Cookbook.Repository.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Cookbook.Repository.Repositories
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IRecipebookRepository, RecipebookRepository>();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory(connectionString));

            return services;
        }
    }
}
