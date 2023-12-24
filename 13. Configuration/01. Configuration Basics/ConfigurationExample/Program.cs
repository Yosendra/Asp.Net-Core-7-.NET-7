
// Configuration Setting
//   Configuration (configuration setting) are the constant key/value pairs that
//   are set at a common location and can be read from anywhere in the same application.
//
//   Examples: connection string, Client ID & API keys to make REST-API calls,
//             Domain names, Constant email addresses, etc
//
//   Controller ------------>   appsettings.json
//                              { "key" : "value" }
//   Services -------------->
//
//   Program.cs ------------>
//
// Look at : appsettings.json, Program.cs, 
//
// Configuration Sources:
//  - appsettings.json
//  - Environment Variables
//  - File Configuration (json, ini, xml files)
//  - In-Memory Configuration
//  - Secret Manager

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.Map("/", async context =>
    {
        // Read configuration
        await context.Response.WriteAsync(app.Configuration["MyKey"] + "\n");
        await context.Response.WriteAsync(app.Configuration.GetValue<string>("MyKey") + "\n");
        await context.Response.WriteAsync(app.Configuration.GetValue<int>("x", 10) + "\n");
    });
});
app.MapControllers();
app.Run();
