
// Strongly Typed Partial Views
//   is a partial view that is bound to a specified model class.
//   So, it gets all the benefit of a strongly typed view.
//
//        Partial View       @model ModelClassName        Model
//     Presentation Logic  -------------------------> Data Properties
//   & @Model.PropertyName


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
