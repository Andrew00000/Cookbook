using Cookbook.API.Mapping;
using Cookbook.Application.Database;
using Cookbook.Application.Services;
using Cookbook.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipebookReadService recipebookReadServices;
        private readonly IRecipebookWriteService recipebookWriteServices;

        public RecipesController(IRecipebookReadService recipebookReadServices, 
                                 IRecipebookWriteService recipebookWriteServices)
        {
            this.recipebookReadServices = recipebookReadServices;
            this.recipebookWriteServices = recipebookWriteServices;
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CreateRecipeRequest request,
                                                CancellationToken token)
        {
            //if (!ModelState.IsValid)
            //{
            //return BadRequest(ModelState);
            //}

            var recipe = request.MapToRecipe();

            await recipebookWriteServices.CreateAsync(recipe, token);

            var response = recipe.MapToResponse();

            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> Get([FromRoute] string slug, CancellationToken token)
        {
            var recipe = await recipebookReadServices.GetBySlugAsync(slug, token);

            if (recipe is null)
            {
                return NotFound();
            }

            var response = recipe.MapToResponse();
            return Ok(response);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get([FromRoute] long id, CancellationToken token)
        {
            var recipe = await recipebookReadServices.GetByIdAsync(id, token);

            if (recipe is null)
            {
                return NotFound();
            }

            var response = recipe.MapToResponse();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var recipes = await recipebookReadServices.GetAllAsync(token);

            var responses = recipes.MapToResponse();

            return Ok(responses);
        }

        [HttpGet("titles")]
        public async Task<IActionResult> GetAllTitles(CancellationToken token)
        {
            var recipeTitles = await recipebookReadServices.GetAllTitlesAsync(token);
            return Ok(recipeTitles);
        }

        [HttpGet("titles/tags/{tag}")]
        public async Task<IActionResult> GetAllWithTag([FromRoute] string tag,
                                                       CancellationToken token)
        {
            var recipeTitles = await recipebookReadServices
                                        .GetAllTitlesWithTagAsync(tag, token);

            return Ok(recipeTitles);
        }

        [HttpPut("{idOrSlug}")]
        public async Task<IActionResult> Update([FromRoute] string idOrSlug, 
                                                [FromBody] UpdateRecipeRequest request,
                                                CancellationToken token)
        {
            var recipe = Int64.TryParse(idOrSlug, out var id)
                            ? request.MapToRecipe(id)
                            : request.MapToRecipe(
                                await recipebookReadServices
                                            .GetIdFromSlugAsync(idOrSlug, token));

            if (recipe is null)
            {
                return NotFound();
            }

            var updatedRecipe = await recipebookWriteServices.UpdateByIdAsync(recipe, token);

            if (updatedRecipe is null)
            {
                return NotFound();
            }

            var response = updatedRecipe.MapToResponse();
            return Ok(response);
        }

        [HttpDelete("{idOrSlug}")]
        public async Task<IActionResult> Delete([FromRoute] string idOrSlug,
                                                CancellationToken token)
        {
            var deleted = Int64.TryParse(idOrSlug, out var id)
                            ? await recipebookWriteServices.DeleteByIdAsync(id, token)
                            : await recipebookWriteServices.DeleteBySlugAsync(idOrSlug, token);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
