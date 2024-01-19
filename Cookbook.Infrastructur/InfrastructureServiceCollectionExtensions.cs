using Cookbook.Application;
using Cookbook.Application.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Cookbook.Infrastructur
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddSingleton<ICookbookRepository, CookbookRepository>();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory());

            return services;
        }
    }
}
