using Cookbook.Application.Database;
using Cookbook.Repository.Database;
using Cookbook.Repository.Database.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cookbook.Repository.Repositories
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IRecipebookRepository, RecipebookRepository>();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseConfiguration>(configuration.GetSection("Database"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<DatabaseConfiguration>>().Value);
            services.AddSingleton<IDbConnectionFactory, SqliteConnectionFactory>();
            services.AddSingleton<IDbManipulator, DbManipulator>();
            services.AddSingleton<IDbInitializer, DbInitializer>();

            return services;
        }
    }
}
