using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters;

public class ResponseHeaderActionFilter : IActionFilter
{
    private readonly ILogger<ResponseHeaderActionFilter> _logger;
    private readonly string Key;
    private readonly string Value;

    public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger,
        string key,
        string value)
    {
        _logger = logger;
        Key = key;
        Value = value;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("{FilterName}.{MethodName} method",
            nameof(PersonListActionFilter),
            nameof(OnActionExecuting));

        // Adding new key-value into the response header
        context.HttpContext.Response.Headers[Key] = Value;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("{FilterName}.{MethodName} method",
            nameof(PersonListActionFilter),
            nameof(OnActionExecuted));
    }
}
