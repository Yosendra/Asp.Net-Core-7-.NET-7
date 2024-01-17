using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;

/* EF CRUD Operation
 * 
 *   SELECT - SQL
 *     SELECT column_1, column_2 FROM table_name
 *     WHERE column_name = value
 *     ORDER BY column_name
 *   LINQ Query Version
 *     _dbContext.[DbSetName]
 *       .Where(item => item.Property == value)     // Specifies condition for where clause
 *       .OrderBy(item => item.Property)            // Specifies condition for 'order by' clause
 *       .Select(item => item)                      // Expression to be executed for each row
 *       
 *   INSERT - SQL
 *     INSERT INTO table_name(column_1, column_2)
 *     VALUES(value_1, value2)
 *   LINQ Query Version
 *     _dbContext.[DbSetName].Add(entity_object);   // Adds the given model object (entity object) to the DbSet
 *     _dbContext.SaveChanges();                    // Generate the SQL INSERT statement based on the model object data and
 *                                                      executes the same at database server
 *   
 *   DELETE - SQL
 *     DELETE FROM table_name WHERE contidition
 *   LINQ Query Version
 *     _dbContext.[DbSetName].Add(entity_object);   // Removes the specified model object (entity object) from the DbSet
 *     _dbContext.SaveChanges();                    // Generate the SQL DELETE statement based on the model object data and
 *                                                      executes the same at database server
 *                                                      
 *   UPDATE - SQL
 *     UPDATE table_name SET column_name1 = value, column_name2 = value WHERE primary_key_column = value
 *   LINQ Query Version
 *     entityObject.Property = value;               // Update the specified value in the specific property of the model object (entity object) in DbSet
 *     _dbContext.SaveChanges();                    // Generate the SQL UPDATE statement based on the model object data and
 *                                                      executes the same at database server
 *   
 *   Service by AddDbContext<PersonsDbContext> is setted by EFCore to 'Scoped', so we need to change our service lifetime to Scoped also
 *   
 *   Look at: CountryService.cs, PersonService.cs
 *            Dependency Injection change to 'Scope' which before was 'Singleton'
 *            ServiceCollectionExtension.cs, change our service lifetime from Singleton to Scoped
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
