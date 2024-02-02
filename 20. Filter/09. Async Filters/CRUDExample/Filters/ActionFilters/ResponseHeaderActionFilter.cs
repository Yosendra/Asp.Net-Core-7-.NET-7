using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters;

public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter            // notice here, we implements "IAsyncActionFilter" instead of "IActionFilter"
{
    private readonly ILogger<ResponseHeaderActionFilter> _logger;
    private readonly string Key;
    private readonly string Value;

    public int Order { get; set; }

    public ResponseHeaderActionFilter(
        ILogger<ResponseHeaderActionFilter> logger,
        string key,
        string value,
        int order)
    {
        _logger = logger;
        Key = key;
        Value = value;
        Order = order;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)                      // Notice this
    {
        _logger.LogInformation("{FilterName}.{MethodName} method - BEFORE",         // The logic for BEFORE
           nameof(PersonListActionFilter),
           nameof(OnActionExecutionAsync));
        context.HttpContext.Response.Headers[Key] = Value;

        await next();                                                               // calls the subsequent filter or action method

        _logger.LogInformation("{FilterName}.{MethodName} method - AFTER",          // The logic for AFTER
            nameof(ResponseHeaderActionFilter),
            nameof(OnActionExecutionAsync));
    }
}
