using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Cookbook.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(
                                                            ServiceLifetime.Singleton);

            return services;
        }
    }
}
