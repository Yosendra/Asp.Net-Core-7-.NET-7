
// <environment> Tag Helper
//  
//   <environment include="Environment1, Environment2">
//      html content here
//   </environment>
//
//   <environment exclude="Environment1, Environment2">
//      html content here
//   </environment>
//   
// It renders the content only when the current environment name matches
// with either of the specified environment names in the "include" property
// and vice versa for the exclude
//
// case: in case if you are in the development environment, you may show a button
//       for database migration or opening the log, otherwise.
//       in case if you are in the production environment, you may show an additional
//       button for end-user for the database backup
//
// look at index view, ViewImports

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
