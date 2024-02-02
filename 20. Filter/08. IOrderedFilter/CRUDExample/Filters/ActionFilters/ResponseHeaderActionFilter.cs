using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters;

public class ResponseHeaderActionFilter : IActionFilter, IOrderedFilter             // implement "IOrderedFilter"
{
    private readonly ILogger<ResponseHeaderActionFilter> _logger;
    private readonly string Key;
    private readonly string Value;

    public int Order { get; set; }                                                  // implemented from interface's member, see the interface definition

    public ResponseHeaderActionFilter(
        ILogger<ResponseHeaderActionFilter> logger,
        string key,
        string value,
        int order)                                                                  // inject the value of Order in constructor
    {
        _logger = logger;
        Key = key;
        Value = value;
        Order = order;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("{FilterName}.{MethodName} method",
            nameof(PersonListActionFilter),
            nameof(OnActionExecuting));

        context.HttpContext.Response.Headers[Key] = Value;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("{FilterName}.{MethodName} method",
            nameof(PersonListActionFilter),
            nameof(OnActionExecuted));
    }
}
