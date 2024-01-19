using Cookbook.Application.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Cookbook.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory());

            return services;
        }
    }
}
