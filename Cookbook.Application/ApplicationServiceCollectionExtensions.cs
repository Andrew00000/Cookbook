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
            services.AddSingleton<IRecipebookReadService, RecipebookReadService>();
            services.AddSingleton<IRecipebookWriteService, RecipebookWriteService>();

            return services;
        }
    }
}
