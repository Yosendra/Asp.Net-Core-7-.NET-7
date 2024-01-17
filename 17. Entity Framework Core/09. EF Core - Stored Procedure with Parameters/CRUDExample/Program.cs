using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;

/* EF Core - Stored Procedure with Parameters
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
 *   First add the SP through Migration.
 *   
 *   How to create Store Procedure throug code (migration)?
 *      In Package Manager Console type: Add-Migration InsertPerson_StoredProcedure
 *   
 *   Create SP with Parameters
 *     CREATE PROCEDURE [schema].[procedure_name] (@paramater_one data_type, @paramater_two data_type)
 *     AS BEGIN
 *       statement...
 *     END
 *   
 *   Take a look at two methods generated from Add-Migration command at Migration file. There are Up() and Down()
 *   Whenever you put command Update-Database, the Up() is executed, when you want to rollback, then the Down() method is executed. 
 *   
 *   Look at:
 *     20240117130320_InsertPerson_StoredProcedure.cs, we write our SP statment inside Up() method
 *     PersonService.cs at AddPerson() method
 *     PersonsDbContext.cs at 
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
