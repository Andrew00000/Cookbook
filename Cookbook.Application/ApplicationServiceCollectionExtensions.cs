using Cookbook.Application.Services;
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
            services.AddSingleton<IRecipebookReadServices, RecipebookReadServices>();
            services.AddSingleton<IRecipebookWriteServices, RecipebookWriteServices>();

            return services;
        }
    }
}
