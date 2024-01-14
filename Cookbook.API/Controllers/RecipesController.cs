using Cookbook.API.Mapping;
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
            var recipe = request.MapToRecipe();

            await cookbookRepository.CreateAsync(recipe);

            return Created($"/api/recipes/{recipe.Id}", recipe.MapToResponse()); 
        }

        
    }
}
