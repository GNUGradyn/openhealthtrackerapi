using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OpenHealthTrackerApi.Pipeline;

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is HttpAccessDeniedException _)
        {
            context.Result = new ObjectResult("Access to the resource was denied")
            {
                StatusCode = 403
            };
            context.ExceptionHandled = true;
        }
        if (context.Exception is HttpNotFoundExeption _)
        {
            context.Result = new ObjectResult("Resource not found")
            {
                StatusCode = 404
            };
            context.ExceptionHandled = true;
        }
    }
}