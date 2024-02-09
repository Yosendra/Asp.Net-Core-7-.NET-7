using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResultFilters;

public class PersonListResultFilter : IAsyncResultFilter
{
    private readonly ILogger<PersonListResultFilter> _logger;

    public PersonListResultFilter(ILogger<PersonListResultFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        // BEFORE
        _logger.LogInformation("{FilterName}.{MethodName} - BEFORE", 
            nameof(PersonListResultFilter), 
            nameof(OnResultExecutionAsync));

        // Add the our custom response header here
        context.HttpContext.Response.Headers["Last-Modified"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

        await next();   // call the subsequent filter [or] IActionResult

        // AFTER
        _logger.LogInformation("{FilterName}.{MethodName} - AFTER",
            nameof(PersonListResultFilter),
            nameof(OnResultExecutionAsync));

        // Try to put this logic here, but it doesn't work. Therefore I put the logic in BEFORE
        //context.HttpContext.Response.Headers["Last-Modified"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
    }
}
