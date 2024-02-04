using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ExceptionFilters;

public class HandleExceptionFilter : IExceptionFilter
{
    private readonly ILogger<HandleExceptionFilter> _logger;
    private readonly IHostEnvironment _hostEnvironment;             // notice this, to get the environment name

    public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger, IHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError("Exception filter {FilterName}.{MethodName}\n{ExceptionType}\n{ExceptionMessage}",
            nameof(HandleExceptionFilter),
            nameof(OnException),
            context.Exception.GetType().ToString(),
            context.Exception.Message);

        // Differentiate exception message depends on the environment where the user in
        if (_hostEnvironment.IsDevelopment())
        {
            // Short-Circuit
            context.Result = new ContentResult()
            {
                // return the plain text contain the exception message to the page
                Content = context.Exception.Message,
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        
    }
}
