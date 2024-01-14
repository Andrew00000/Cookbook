using Cookbook.Application;
using Cookbook.Contracts.Requests;
using Cookbook.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Cookbook.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class RecipesController : ControllerBase
    {
        private readonly ICookbookRepository cookbookRepository;

        public RecipesController(ICookbookRepository cookbookRepository)
        {
            this.cookbookRepository = cookbookRepository;
        }

        [HttpPost("recipes")]
        public async Task<IActionResult> Create([FromBody] CreateRecipeRequest request)
        {
            var recipe = new Recipe
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Author = request.Author,
                Tags = request.Tags,
                Portions = request.Portions,
                Calories = request.Calories,
                Ingredients = request.Ingredients.Select(x => CreateIngredient(x)),
                Steps = request.Steps,
            };

            var result = await cookbookRepository.CreateAsync(recipe);

            return Ok(recipe); //should be recipeResponse
        }

        private Ingredient CreateIngredient(string rawIngredient)
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
                    Name = name }
                ;
            }
            else
            {
                throw new ArgumentException($"Invalid ingredient: {rawIngredient}");
            }
        }
    }
}
