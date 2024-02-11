using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.AuthorizationFilters;

public class TokenAuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Verify is token submitted in the form of a cookie as a part of the request
        if (!context.HttpContext.Request.Cookies.ContainsKey("Auth-Key"))
        {
            // Unauthorized, Short-circuit the pipeline
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            return;
        }

        if (context.HttpContext.Request.Cookies["Auth-Key"] != "A100")
        {
            // Unauthorized, Short-circuit the pipeline
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            return;
        }
    }
}
