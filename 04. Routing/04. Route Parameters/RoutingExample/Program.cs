
// Route Parameters

//  /files/sample.txt       ->   /files/{filename}.{extension}
//  /employee/profile/john  ->   /employee/profile/{employeeName}

//      the 'files' is static, sample.txt can be vary. The vary part we call 'Route Parameter'
//      whichever name outside of the curly braces we call literal text, if inside, we call it parameter


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    // /files/sampletxt (remove ., without extension)   -> doesn't match
    // /file/sample.txt (remove s)                      -> doesn't match
    endpoints.Map("files/{filename}.{extension}", async (context) =>
    {
        // we can access the route parameter value in property called 'Route Value'
        string? filename = Convert.ToString(context.Request.RouteValues["filename"]);
        string? extension = Convert.ToString(context.Request.RouteValues["extension"]);

        await context.Response.WriteAsync($"In files - {filename} - {extension}");
    });

    endpoints.Map("employee/profile/{employee}", async (context) =>
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
