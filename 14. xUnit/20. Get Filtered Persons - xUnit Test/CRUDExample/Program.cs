
// Look at: PersonAddRequest.cs, PersonResponse.cs, Person.cs, GenderOption.cs, PersonExtension class
//          IPersonService.cs, PersonService.cs (GetFilteredPersons)
//          ValidationHelper

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
