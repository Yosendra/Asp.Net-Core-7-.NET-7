using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;

/* EF Core - Stored Procedure
 *   If you want to perform multiple sql statement or complex database operation through a single database call
 *   then creating stored procedure in the database is the best choice
 *   
 *   Calling Stored Procedure
 *     For INSERT, UPDATE, DELETE   
 *       int DbContext.Database.ExecuteSqlRaw(
 *         string sql,                              // Eg: "EXECUTE [dbo].[store_procedure_name] @param1 @param2"
 *         params object[] parameters               // A list of objects of SqlParameter type
 *       )
 *       
 *     For SELECT   
 *       IQueryable<Model> [DbSetName].FromSqlRaw(
 *         string sql,                              // Eg: "EXECUTE [dbo].[store_procedure_name] @param1 @param2"
 *         params object[] parameters               // A list of objects of SqlParameter type
 *       )
 *   
 *   How to create Store Procedure throug code?
 *      In Package Manager Console type: Add-Migration GetPersons_StoredProcedure
 *   
 *   Take a look at two methods generated from Add-Migration command at Migration file. There are Up() and Down()
 *   Whenever you put command Update-Database, the Up() is executed, when you want to rollback, then the Down() method is executed. 
 *   
 *   Look at: CountryServiceTest.cs, PersonServiceTest.cs some changes in the constructor
 *            20240117114153_GetPersons_StoredProcedure.cs we are going to write our SP here inside Up() method
 *            PersonsDbContext.cs we define a method sp_GetAllPersons() to call the store procedure we have created
 *            PersonService.cs at GetAllPersons() method, we use the SP
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
