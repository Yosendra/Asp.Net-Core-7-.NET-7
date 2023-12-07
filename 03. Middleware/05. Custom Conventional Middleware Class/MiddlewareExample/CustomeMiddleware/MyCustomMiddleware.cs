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

    public static class CustomMiddleWareExtension
    {
        public static IApplicationBuilder UseMyCustomMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MyCustomMiddleware>();
        }
    }
}
