﻿using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;

/* Async EF Operation
 *   async
 *     The method is awaitable.
 *     Can execute I/O bound code or CPU-bound code.
 *   await
 *     Waits for the I/O bound or CPU-bound code execution gets completed
 *     After completion, it returns the return value
 *   
 *   Insight: most I/O operation (access database or external resources) must be done in asynchronous operation.
 *     
 *   Look at: ICountryService.cs, IPersonService.cs, change all method to into asynchronous
 *            CountryService.cs, PersonService.cs, we implement the methods into Task based and using async version and adding await keyword
 *            
 *   On this part controllers has some error due to we change the service interface
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
