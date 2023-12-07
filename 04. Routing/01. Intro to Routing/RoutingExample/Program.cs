var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Enables routing and select an approriate endpoint based on the url path and HTTP method
app.UseRouting();   // This enable the routing. It only the endpoint, not execute the endpoint

// Creating endpoints
// Execute the approriate endpoint based on the endpoint selected by the above UseRouting() method
app.UseEndpoints(endpoints =>
{
    // Add your end points here
    //endpoints.MapControllers();
    //endpoints.MapPost();
    //endpoints.MapGet();
});

app.Run();
