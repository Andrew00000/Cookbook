using Cookbook.API.Mapping;
using Cookbook.Application.Database;
using Cookbook.Application.Services;
using Cookbook.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.API.Controllers
{
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipebookReadServices recipebookReadServices;
        private readonly IRecipebookWriteServices recipebookWriteServices;

        public RecipesController(IRecipebookReadServices recipebookReadServices, 
                                 IRecipebookWriteServices recipebookWriteServices)
        {
            this.recipebookReadServices = recipebookReadServices;
            this.recipebookWriteServices = recipebookWriteServices;
        }

        [HttpPost(ApiEndPoints.Recipes.Create)]
        public async Task<IActionResult> Create([FromBody] CreateRecipeRequest request,
                                                CancellationToken token)
        {
            var recipe = request.MapToRecipe();

            await recipebookWriteServices.CreateAsync(recipe, token);

            var response = recipe.MapToResponse();

            return CreatedAtAction(nameof(Get), new { idOrSlug = response.Id }, response);
        }

        [HttpGet(ApiEndPoints.Recipes.Get)]
        public async Task<IActionResult> Get([FromRoute] string idOrSlug,
                                                CancellationToken token)
        {
            var recipe = Guid.TryParse(idOrSlug, out var id)
                            ? await recipebookReadServices.GetByIdAsync(id, token)
                            : await recipebookReadServices.GetBySlugAsync(idOrSlug, token);

            if (recipe is null)
            {
                return NotFound();
            }

            var response = recipe.MapToResponse();
            return Ok(response);
        }

        [HttpGet(ApiEndPoints.Recipes.GetAll)]
        public async Task<IActionResult> GetAll(CancellationToken token)
        {
            var recipes = await recipebookReadServices.GetAllAsync(token);

            var responses = recipes.MapToResponse();

            return Ok(responses);
        }

        [HttpGet(ApiEndPoints.Recipes.GetAllTitles)]
        public async Task<IActionResult> GetAllTitles(CancellationToken token)
        {
            var recipeTitles = await recipebookReadServices.GetAllTitlesAsync(token);
            return Ok(recipeTitles);
        }

        [HttpGet(ApiEndPoints.Recipes.GetAllTitlesWithTag)]
        public async Task<IActionResult> GetAllWithTag([FromRoute] string tag,
                                                       CancellationToken token)
        {
            var recipeTitles = await recipebookReadServices
                                        .GetAllTitlesWithTagAsync(tag, token);

            return Ok(recipeTitles);
        }

        [HttpPut(ApiEndPoints.Recipes.Update)]
        public async Task<IActionResult> Update([FromRoute] string idOrSlug, 
                                                [FromBody] UpdateRecipeRequest request,
                                                CancellationToken token)
        {
            var recipe = Guid.TryParse(idOrSlug, out var id)
                            ? request.MapToRecipe(id)
                            : request.MapToRecipe(
                                await recipebookReadServices
                                            .GetIdFromSlugAsync(idOrSlug, token));

            var updatedRecipe = await recipebookWriteServices.UpdateByIdAsync(recipe, token);

            if (updatedRecipe is null)
            {
                return NotFound();
            }

            var response = updatedRecipe.MapToResponse();
            return Ok(response);
        }

        [HttpDelete(ApiEndPoints.Recipes.Delete)]
        public async Task<IActionResult> Delete([FromRoute] string idOrSlug,
                                                CancellationToken token)
        {
            var deleted = Guid.TryParse(idOrSlug, out var id)
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
