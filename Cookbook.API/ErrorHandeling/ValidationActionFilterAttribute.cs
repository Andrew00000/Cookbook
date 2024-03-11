using Cookbook.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cookbook.API.ErrorHandeling
{
    public class ValidationActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var validationFailureResponse = new ValidationFailureResponse
                {
                    Errors = context.ModelState
                        .Where(x => x.Value.Errors.Any())
                        .Select(x => new ValidationResponse
                        {
                            Message = $"{x.Key}: {x.Value.Errors.First().ErrorMessage}"
                        })
                };

                context.Result = new ObjectResult(validationFailureResponse)
                {
                    StatusCode = 400
                };
            }

            base.OnActionExecuting(context);
        }
    }
}