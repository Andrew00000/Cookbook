using Cookbook.Application.Database;
using Cookbook.Domain.Models;
using FluentValidation;

namespace Cookbook.Application.Validators //is this the right place for validators?
{
    public class RecipeValidator : AbstractValidator<Recipe>
    {
        private readonly IRecipebookRepository recipebookRepository;
        public RecipeValidator(IRecipebookRepository recipebookRepository)
        {
            this.recipebookRepository = recipebookRepository;
            
            RuleFor(x => x.Title).NotEmpty();

            RuleFor(x => x.Author).NotEmpty();

            RuleFor(x => x.NumberOfPortions).GreaterThan(0)
                    .WithMessage("Number of portions have to be larger than 0");

            RuleFor(x => x.Ingredients).NotEmpty();

            RuleFor(x => x.Steps).NotEmpty();

            RuleFor(x => x.Slug)
                    .MustAsync(ValidateSlug)
                    .WithMessage("This recipe already exiests. :(");

        }

        private async Task<bool> ValidateSlug(Recipe recipe, string slug,
                                              CancellationToken token = default)
        {
            var existingRecipe = await recipebookRepository.GetBySlugAsync(slug);

            if (existingRecipe is not null)
            {
                return existingRecipe.Id == recipe.Id;
            }

            return existingRecipe is null;
        }
    }
}
