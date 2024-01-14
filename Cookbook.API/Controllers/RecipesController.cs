using Cookbook.API.Mapping;
using Cookbook.Application;
using Cookbook.Contracts.Requests;
using Cookbook.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Cookbook.API.Controllers
{
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly ICookbookRepository cookbookRepository;

        public RecipesController(ICookbookRepository cookbookRepository)
        {
            this.cookbookRepository = cookbookRepository;
        }

        [HttpPost(ApiEndPoints.Recipes.Create)]
        public async Task<IActionResult> Create([FromBody] CreateRecipeRequest request)
        {
            var recipe = request.MapToRecipe();

            await cookbookRepository.CreateAsync(recipe);

            var response = recipe.MapToResponse();

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpGet(ApiEndPoints.Recipes.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var recipe = await cookbookRepository.GetByIdAsync(id);

            if (recipe is null)
            {
                return NotFound();
            }

            var response = recipe.MapToResponse();
            return Ok(response);
        }

        [HttpGet(ApiEndPoints.Recipes.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var recipes = await cookbookRepository.GetAllAsync();

            var responses = recipes.MapToResponse();

            return Ok(responses);
        }

        [HttpPut(ApiEndPoints.Recipes.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody]UpdateRecipeRequest request)
        {
            var recipe = request.MapToRecipe(id); 
            var updated = await cookbookRepository.UpdateByIdAsync(recipe);

            if(!updated)
            {
                return NotFound();
            }

            var response = recipe.MapToResponse();
            return Ok(response);
        }
    }
}
