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
                Tags = request.Tags,
                Portions = request.Portions,
                Calories = request.Calories,
                Ingredients = request.Ingredients.Select(CreateIngredient),
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
                Tags = request.Tags,
                Portions = request.Portions,
                Calories = request.Calories,
                Ingredients = request.Ingredients.Select(CreateIngredient),
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
                Portions = recipe.Portions,
                Calories = recipe.Calories,
                Ingredients = recipe.Ingredients.Select(IngredientToString),
                Steps = recipe.Steps,
            };
        }

        public static RecipesResponse MapToResponse(this IEnumerable<Recipe> recipes)
            => new(){ Recipes = recipes.Select(MapToResponse) };

        private static Ingredient CreateIngredient(string rawIngredient)
        {
            var splitIngredient = rawIngredient.Split(' ');
            var volume = int.Parse(splitIngredient[0]);
            var name = splitIngredient[2];
            if (Enum.TryParse<UnitType>(splitIngredient[1], ignoreCase: true, out UnitType unit))
            {
                return new Ingredient
                {
                    Volume = volume,
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
            => $"{ingredient.Volume} {ingredient.Unit} {ingredient.Name}";
    }
}
