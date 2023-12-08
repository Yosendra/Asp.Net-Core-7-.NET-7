var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();   // This enable the routing. It only the endpoint, not execute the endpoint

// Creating endpoints
// Execute the approriate endpoint based on the endpoint selected by the above UseRouting() method
app.UseEndpoints(endpoints =>
{
    // Add your end points here
    //endpoints.MapControllers();
    //endpoints.MapPost();
    //endpoints.MapGet();
    //endpoints.Map();

    // Whenever middleware executed based on the routing it is called endpoints

    // .Map() works for all HTTP methods, either POST or GET or etc
    endpoints.Map("/map1", async (context) =>   // It doesn't forward the request to the subsequent middleware. It stop here
    {
        await context.Response.WriteAsync("In Map 1");
    });
    
    endpoints.Map("/map2", async (context) =>   // It doesn't forward the request to the subsequent middleware. It stop here
    {
        await context.Response.WriteAsync("In Map 2");
    });
    
    // .MapGet() work only for a request with method 'GET'
    endpoints.MapGet("/mapget", async (context) =>   // It doesn't forward the request to the subsequent middleware. It stop here
    {
        await context.Response.WriteAsync("In Map GET");
    });

    // .MapPost() work only for a request with method 'POST'
    endpoints.MapPost("/mappost", async (context) =>   // It doesn't forward the request to the subsequent middleware. It stop here
    {
        await context.Response.WriteAsync("In Map POST");
    });
});

app.Run(async context =>    // This will be executed for the path that other the path we have set in routing
{
    await context.Response.WriteAsync($"Request received at {context.Request.Path}");
});

app.Run();
