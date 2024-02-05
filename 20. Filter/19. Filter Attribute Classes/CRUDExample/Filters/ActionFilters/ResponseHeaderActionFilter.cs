using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters;

//public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
public class ResponseHeaderActionFilter : ActionFilterAttribute                                 // inherit ActionFilterAttribute
{
    //private readonly ILogger<ResponseHeaderActionFilter> _logger;                             // filter attribute doesn't support constructor DI
    private readonly string Key;
    private readonly string Value;

    //public int Order { get; set; }                                                            // this is not needed anymore because it is already defined in parent     

    public ResponseHeaderActionFilter(
        //ILogger<ResponseHeaderActionFilter> logger,                                           // filter attribute doesn't support constructor DI
        string key,
        string value,
        int order)
    {
        //_logger = logger;                                                                     // filter attribute doesn't support constructor DI
        Key = key;
        Value = value;
        Order = order;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)         // adding override modifier
    {
        //_logger.LogInformation("{FilterName}.{MethodName} method - BEFORE",
        //   nameof(PersonListActionFilter),
        //   nameof(OnActionExecutionAsync));
        context.HttpContext.Response.Headers[Key] = Value;

        await next();

        //_logger.LogInformation("{FilterName}.{MethodName} method - AFTER",
        //    nameof(ResponseHeaderActionFilter),
        //    nameof(OnActionExecutionAsync));
    }
}
