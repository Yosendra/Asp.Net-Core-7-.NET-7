namespace MiddlewareExample.CustomeMiddleware
{
    public class MyCustomMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("\nMy Custom Middleware - Starts");
            await next(context);
            await context.Response.WriteAsync("\nMy Custom Middleware - Ends");
        }
    }

    // We create our middleware extension method
    public static class CustomMiddleWareExtension
    {
        // By convention, it is better to use prefix 'Use' in the custom middleware name
        // The return type is 'IApplicationBuilder', I follow the usual return type in built-in extension middleware method
        // I've tested with void and still working.
        public static IApplicationBuilder UseMyCustomMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MyCustomMiddleware>();
        }
    }
}
