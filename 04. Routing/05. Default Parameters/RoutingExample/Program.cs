
//  Default Router Parameter -> {parameter=default_value}
//   A route parameter with default value matches with any value.
//   It also matches with empty value. In this case, the default value will be
//   considered into the parameter.

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

    // '{employee=yosi}' is how we give default value in Route Parameter
    // try to access path : /employee/profile/
    // output: 'In employee profile - yosi'
    endpoints.Map("employee/profile/{employee=yosi}", async (context) =>
    {
        string? employee = Convert.ToString(context.Request.RouteValues["employee"]);

        await context.Response.WriteAsync($"In employee profile - {employee}");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"\nRequest received at {context.Request.Path}");
});

app.Run();
