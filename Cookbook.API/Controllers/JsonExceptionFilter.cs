using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Cookbook.Contracts.Responses;

namespace Cookbook.API.Controllers
{
    public class JsonExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is not null)
            {
                var validationFailureResponse = new ValidationFailureResponse
                {
                    Errors = new List<ValidationResponse>
                    {
                        new ValidationResponse
                        {
                            Message = "Error deserializing JSON:" + context.Exception.Message
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
    }
}
