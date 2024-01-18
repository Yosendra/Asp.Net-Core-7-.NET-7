using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;

/* EF - Table Rfelation with Fluent API
 * 
 * Master Model class
 * public class MasterModel
 * {
 *   public data_type PropertyName {get; set;}
 *   public virtual ICollection<ChildModel> ChildPropertyName {get; set;}
 * }
 * 
 * Child Model class
 * public class ChildModel
 * {
 *   public data_type PropertyName {get; set;}
 *   public virtual ParentModel ParentPropertyName {get; set;}
 * }
 * 
 *   Look at: Person.cs, Country.cs at bottom, adding navigation property
 *            PersonService.cs, using Include() to get the child model
 *            PersonDbContext.cs to see how to make relation through migration (but cancelled, recommended using attribute. Check Person.cs)
 *            ConvertPersonToPersonResponse() we don't need to call CountryService since now we have navigation property of Country in Person
 *            Changes using extension method to convert Person to PersonResponse instead of private method, adding CountryName
 */

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddDbContext<PersonsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddServices()
    .AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.MapControllers();
app.Run();
