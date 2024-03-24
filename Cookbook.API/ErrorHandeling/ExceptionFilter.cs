using Cookbook.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cookbook.API.ErrorHandeling
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is not null)
            {
                var errorMessage = RemoveWhitespaceCharacters(context.Exception.Message);
                var validationFailureResponse = new ValidationFailureResponse
                {
                    Errors = new List<ValidationResponse>
                    {
                        new ValidationResponse
                        {
                            Message = $"*sad noises*: {errorMessage}"
                        }
                    }
                };

                context.Result = new ObjectResult(validationFailureResponse)
                {
                    StatusCode = 400
                };

                context.ExceptionHandled = true;
            }
        }

        private string RemoveWhitespaceCharacters(string input)
            => input.Replace("\r\n", string.Empty);
    }
}
