
// Look at: PersonUpdateRequest.cs, PersonResponse.cs, Person.cs
//          IPersonService.cs, PersonService.cs (UpdatePerson)

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
