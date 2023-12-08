
// Route Constraint -> {parameter:data_type}
// data_type :
//  int      -> {id:int}        123456789, -123456789
//  bool     -> {active:bool}   true, false, TRUE, FALSE (case insensitive)
//  datetime -> {date:datetime} "yyyy-MM-dd hh:mm:ss tt" or "MM/dd/yyyy hh:mm:ss tt"

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

    // Notice the {id:int?}, if we not supplied integer it will run to the next route or middleware
    endpoints.Map("products/details/{id:int?}", async (context) =>
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

    // Notice the {reportdate:datetime}
    // try to go to this url 'daily-digest-report/2023-08-17'
    // Output: In daily-digest-report - 8/17/2023
    endpoints.Map("daily-digest-report/{reportdate:datetime}", async (context) =>
    {
        DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
        await context.Response.WriteAsync($"In daily-digest-report - {reportDate.ToShortDateString()}");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"\nRequest received at {context.Request.Path}");
});

app.Run();
