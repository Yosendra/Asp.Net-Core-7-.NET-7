
// IConfiguration in Controller
//   by inject IConfiguration interface in the constructor
// 
//
// Look at: HomeController.cs, index view

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
    endpoints.Map("config", async context =>
    {
        await context.Response.WriteAsync(app.Configuration["MyKey"] + "\n");
        await context.Response.WriteAsync(app.Configuration.GetValue<string>("MyKey") + "\n");
        await context.Response.WriteAsync(app.Configuration.GetValue<int>("x", 10) + "\n");
    });
});
app.MapControllers();
app.Run();
