
// Custom Route Constraint Class
// Look at MonthsCustomConstraint.cs

using RoutingExample.CustomConstraints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => 
{
    // Register our custom constraint class to the Services
    options.ConstraintMap.Add("months", typeof(MonthsCustomConstraint));
});

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

    endpoints.Map("employee/profile/{employee:minlength(3)=yosi}", async (context) =>
    {
        string? employee = Convert.ToString(context.Request.RouteValues["employee"]);

        await context.Response.WriteAsync($"In employee profile - {employee}");
    });

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

    endpoints.Map("daily-digest-report/{reportdate:datetime}", async (context) =>
    {
        DateTime reportDate = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
        await context.Response.WriteAsync($"In daily-digest-report - {reportDate.ToShortDateString()}");
    });

    endpoints.Map("cities/{cityid:guid}", async (context) =>
    {
        Guid cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityid"])!);
        await context.Response.WriteAsync($"City information - {cityId}");
    });

    // Look at route parameter 'month', we put constraint 'months' intead of writing it in regex
    // Go to '/sales-report/2020/apr' 
    endpoints.Map("sales-report/{year:int:min(1900)}/{month:months}", async (context) =>
    {
        int year = Convert.ToInt32(context.Request.RouteValues["year"]);
        string? month = Convert.ToString(context.Request.RouteValues["month"]);

        await context.Response.WriteAsync($"sales report - {year} - {month}");
    });

});

app.Run(async context =>
{
    await context.Response.WriteAsync($"\nNo route match at {context.Request.Path}");
});

app.Run();
