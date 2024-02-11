using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResultFilters;

public class TokenResultFilter : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        // this reference to the specification we decide in TokenAuthorizationFilter
        // We use "Auth-Key" as the key for authorization requirement
        context.HttpContext.Response.Cookies.Append("Auth-Key", "A100");
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        
    }
}
