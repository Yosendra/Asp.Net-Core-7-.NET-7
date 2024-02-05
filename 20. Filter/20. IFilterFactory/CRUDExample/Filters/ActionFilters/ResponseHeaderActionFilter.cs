using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters;

// New class that implements IFilterFactory
public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
{
    private string Key { get; set; }
    private string Value { get; set; }
    private int Order { get; set; }

    // True means filter object can be accessible across multiple request in the application
    public bool IsReusable => false;

    public ResponseHeaderFilterFactoryAttribute(
        string key,
        string value,
        int order)
    {
        Key = key;
        Value = value;
        Order = order;
    }

    // Controller -> FilterFactory -> Filter
    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        // return filter object

        //var filter = new ResponseHeaderActionFilter(Key, Value, Order);
        var filter = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();      // get the filter object through IoC container instead, remember to register it in IoC container

        // This is the way we can give our constructor argument to the filter, that's why we make it public property ResponseHeaderActionFilter
        filter.Key = Key;
        filter.Value = Value;
        filter.Order = Order;

        return filter;
    }
}


//public class ResponseHeaderActionFilter : ActionFilterAttribute
public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter            // notice this
{
    //private readonly string Key;
    //private readonly string Value;
    
    // Make these all property
    public string Key { get; set; }
    public string Value { get; set; }
    public int Order { get; set; }

    private readonly ILogger<ResponseHeaderActionFilter> _logger;

    //public ResponseHeaderActionFilter(
    //    string key,
    //    string value,
    //    int order)
    //{
    //    Key = key;
    //    Value = value;
    //    Order = order;
    //}

    // Now we can inject our service though constructor DI
    public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // BEFORE
        _logger.LogInformation("BEFORE - ResponseHeaderActionFilter");

        context.HttpContext.Response.Headers[Key] = Value;

        await next();

        // AFTER
    }
}
