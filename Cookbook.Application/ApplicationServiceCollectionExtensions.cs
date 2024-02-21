using Cookbook.Application.Services;
using Cookbook.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Cookbook.Application
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<RecipeValidator>(
                                                            ServiceLifetime.Singleton);
            services.AddSingleton<IRecipebookReadService, RecipebookService>();
            services.AddSingleton<IRecipebookWriteService, RecipebookService>();

            return services;
        }
    }
}
