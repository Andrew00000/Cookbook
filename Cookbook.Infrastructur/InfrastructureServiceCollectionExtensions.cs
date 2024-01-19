using Cookbook.Application;
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
    }
}
