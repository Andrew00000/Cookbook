using Cookbook.Contracts.Requests;
using Cookbook.Contracts.Responses;
using Cookbook.Domain.Models;

namespace Cookbook.API.Mapping
{
    public static class ContractMapping
    {
        public static Recipe MapToRecipe(this CreateRecipeRequest request)
        {
            return new Recipe
            {
                Title = request.Title,
                Author = request.Author,
                Tags = request.Tags.Select(x => x.ToLower()),
                NumberOfPortions = request.NumberOfPortions,
                Calories = request.Calories,
                Ingredients = request.Ingredients,
                Steps = request.Steps,
            };
        }

        public static Recipe MapToRecipe(this UpdateRecipeRequest request, long id)
        {
            return new Recipe
            {
                Id = id,
                Title = request.Title,
                Author = request.Author,
                Tags = request.Tags.Select(x => x.ToLower()),
                NumberOfPortions = request.NumberOfPortions,
                Calories = request.Calories,
                Ingredients = request.Ingredients,
                Steps = request.Steps,
            };
        }

        public static RecipeResponse MapToResponse(this Recipe recipe)
        {
            return new RecipeResponse
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Author = recipe.Author,
                Tags = recipe.Tags,
                NumberOfPortions = recipe.NumberOfPortions,
                Calories = recipe.Calories,
                Ingredients = recipe.Ingredients,
                Steps = recipe.Steps,
                Slug = recipe.Slug
            };
        }

        public static RecipesResponse MapToResponse(this IEnumerable<Recipe> recipes)
            => new(){ Recipes = recipes.Select(MapToResponse) };
    }
}
