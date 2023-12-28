
// Introduction to xUnit
//
// xUnit is the free, open source unit testing tool for .NET Framework
// One of the third-party package for unit testing the controllers, services or any other classes in asp.net core
// Easy and extensible. Best to use with a mocking framework calle "Moq"
// 
// Add a project for unit test only
//   1. Right click on solution, add new project
//   2. Choose this category: C# - All Platforms - Test
//   3. Select the xUnit Project
//
// To run the unit test
//   Right-click on the unit test project, click 'Run Tests'
//
// Look at: CRUDTests project  

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.MapControllers();
app.Run();
