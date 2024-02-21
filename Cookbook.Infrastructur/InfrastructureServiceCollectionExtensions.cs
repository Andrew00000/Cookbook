using Cookbook.Application.Database;
using Cookbook.Infrastructur.Persistance;
using Cookbook.Repository.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Cookbook.Infrastructur
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IRecipebookRepository, RecipebookRepository>();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionFactory, SqliteConnectionFactory>();

            return services;
        }
    }
}
