using Cookbook.Contracts.Responses;
using FluentValidation;

namespace Cookbook.API.Mapping
{
    public class ValidationMappingMiddleware
    {
        private readonly RequestDelegate next;

        public ValidationMappingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = 400;

                var validationFailureResponse = new ValidationFailureResponse
                {
                    Errors = ex.Errors.Select(x => new ValidationResponse
                    {
                        Message = $@"Something went wrong with {x.PropertyName}: {x.ErrorMessage}"
                    })
                };

                await context.Response.WriteAsJsonAsync(validationFailureResponse);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 400;

                var validationFailureResponse = new ValidationFailureResponse
                {
                    Errors = new[]{ new ValidationResponse
                    {
                        Message = "Something went wrong: \n" + ex.Message
                    }}
                };

                await context.Response.WriteAsJsonAsync(validationFailureResponse);
            }
        }
    }
}
