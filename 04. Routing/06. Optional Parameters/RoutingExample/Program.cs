
// Optional Route Parameter -> {parameter?}
//  '?' indicates an optional parameter. That means, it matches with a value or empty value also.

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.Map("files/{filename}.{extension}", async (context) =>
    {
        string? filename = Convert.ToString(context.Request.RouteValues["filename"]);
        string? extension = Convert.ToString(context.Request.RouteValues["extension"]);

        await context.Response.WriteAsync($"In files - {filename} - {extension}");
    });

    endpoints.Map("employee/profile/{employee=yosi}", async (context) =>
    {
        string? employee = Convert.ToString(context.Request.RouteValues["employee"]);

        await context.Response.WriteAsync($"In employee profile - {employee}");
    });

    // 'id' will be null if the request doesn't supply the id. Notice the '?'
    endpoints.Map("products/details/{id?}", async (context) =>
    {
        if (context.Request.RouteValues.ContainsKey("id"))
        {
            int id = Convert.ToInt32(context.Request.RouteValues["id"]);
            await context.Response.WriteAsync($"Product details - {id}");
        }
        else
        {
            await context.Response.WriteAsync($"Product details - 'id' is not supplied");
        }
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"\nRequest received at {context.Request.Path}");
});

app.Run();
