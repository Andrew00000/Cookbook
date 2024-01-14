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

            return Created($"/{ApiEndPoints.Recipes.Create}/{recipe.Id}", recipe.MapToResponse()); 
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
    }
}
