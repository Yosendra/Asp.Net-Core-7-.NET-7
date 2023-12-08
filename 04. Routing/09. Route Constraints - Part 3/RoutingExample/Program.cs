
// Route Constraint -> {parameter:data_type}
// data_type :
//  int      -> {id:int}        123456789, -123456789
//  bool     -> {active:bool}   true, false, TRUE, FALSE (case insensitive)
//  datetime -> {date:datetime} "yyyy-MM-dd hh:mm:ss tt" or "MM/dd/yyyy hh:mm:ss tt"
//  long     -> {id:long}       123456789, -123456789
//  decimal  -> {price:decimal} 49.88, -1, 0.01
//  guid     -> {id:guid}       Guid (Globally Unique Identifier) value - A hexadecimal number that is universally unique

//  minlength(value)  -> {username:minlength(4)}    Matches with a string that has at least sprecified number of characters
//  maxlength(value)  -> {username:maxlength(4)}    Matches with a string that has or equal to the sprecified number of characters
//  length(min, max)  -> {username:length(4, 7)}    Matches with a string that has number of characters between given minimum and maximum length (both numbers including).
//  range(min, max)   -> {age:rang(18, 100)}        Matches with 18, 19, 99, 100
//  alpha             -> {username:alpha}           Matches with a string that contains only alphabets (A-Z) and (a-z)
//  reges(expression) -> {age:regex(^[0-9]{2}$)}    Matches with 10, 11, 98, 99

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

    // Notice '{employee:minlength(3)=yosi}'
    // We can put second contraint like this '{employee:minlength(3):maxlength(7)=yosi}'
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

    // Notice the constraint we put: '{year:int:min(1900)}' and '{month:regex(^(apr|jul|oct|jan)$)}'
    // Go to url path: /sales-report/2030/apr
    endpoints.Map("sales-report/{year:int:min(1900)}/{month:regex(^(apr|jul|oct|jan)$)}", async (context) =>
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
