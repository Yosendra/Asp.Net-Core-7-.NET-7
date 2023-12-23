
// The logic which is related to the specific domain of the actual client business that is called business logic
//
//			    invoke
// Constroller --------> Service (Business Model)
//   action				 Business Calculation
//			    return	 Business Validation
//			   <-------	 Invoking Data Layer

// Service class is a class that contains business logic such as business calculations, business validation
// that are specific to the domain of the client's business.

// Service is an abstraction layer (middle layer) between presentation layer (or application layer) and data layer.
// It makes the business logic seperated from presentation layer and data layer.
// It makes the business logic to be unit testable easily.
// Always will be invoked by controller.
//
// Any middleware logic is exactly the service

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
