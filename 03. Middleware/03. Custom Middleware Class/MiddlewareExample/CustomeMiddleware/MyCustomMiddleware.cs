
namespace MiddlewareExample.CustomeMiddleware
{
    // We create custom middleware. It is a must to implement IMiddleware interface.
    // After that we need to register this custom middleware to the service
    public class MyCustomMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("\nMy Custom Middleware - Starts");
            await next(context);
            await context.Response.WriteAsync("\nMy Custom Middleware - Ends");
        }
    }
}
