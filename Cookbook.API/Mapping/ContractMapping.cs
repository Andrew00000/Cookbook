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
                Id = Guid.NewGuid(),
                Title = request.Title,
                Author = request.Author,
                Tags = request.Tags.Select(x => x.ToLower()),
                NumberOfPortions = request.NumberOfPortions,
                Calories = request.Calories,
                Ingredients = request.Ingredients.Select(MapToIngredient),
                Steps = request.Steps,
            };
        }

        public static Recipe MapToRecipe(this UpdateRecipeRequest request, Guid id)
        {
            return new Recipe
            {
                Id = id,
                Title = request.Title,
                Author = request.Author,
                Tags = request.Tags.Select(x => x.ToLower()),
                NumberOfPortions = request.NumberOfPortions,
                Calories = request.Calories,
                Ingredients = request.Ingredients.Select(MapToIngredient),
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
                Ingredients = recipe.Ingredients.Select(IngredientToString),
                Steps = recipe.Steps,
                Slug = recipe.Slug
            };
        }

        public static RecipesResponse MapToResponse(this IEnumerable<Recipe> recipes)
            => new(){ Recipes = recipes.Select(MapToResponse) };

        private static Ingredient MapToIngredient(string rawIngredient) //figure out validation so I send back a pretty msg like with recipes
        {
            var splitIngredient = rawIngredient.Split(' ');
            var amount = int.Parse(splitIngredient[0]);
            var name = splitIngredient[2];
            if (Enum.TryParse<UnitType>(splitIngredient[1], ignoreCase: true, out UnitType unit))
            {
                return new Ingredient
                {
                    Amount = amount,
                    Unit = unit,
                    Name = name
                }
                ;
            }
            else
            {
                throw new ArgumentException($"Invalid ingredient: {rawIngredient}");
            }
        }

        private static string IngredientToString(Ingredient ingredient)
            => $"{ingredient.Amount} {ingredient.Unit} {ingredient.Name}";
    }
}
