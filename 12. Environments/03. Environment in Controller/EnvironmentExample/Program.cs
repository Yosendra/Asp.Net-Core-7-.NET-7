
// Access Envrivonment in Controller and other classes
//  - by inject IWebHostEnvironment interface in constructor for controller
//  
// look at HomeController.cs, index view, 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();
 
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.MapControllers();

app.Run();
