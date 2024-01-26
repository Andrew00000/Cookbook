using Cookbook.Application.Database;
using Cookbook.Application.Services;
using Cookbook.Infrastructur.Persistance;
using Cookbook.Infrastructur.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Cookbook.Infrastructur
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddSingleton<IRecipebookRepository, RecipebookRepository>();
            services.AddSingleton<IRecipebookReadServices, RecipebookReadServices>();
            services.AddSingleton<IRecipebookWriteServices, RecipebookWriteServices>();

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory());

            return services;
        }
    }
}
