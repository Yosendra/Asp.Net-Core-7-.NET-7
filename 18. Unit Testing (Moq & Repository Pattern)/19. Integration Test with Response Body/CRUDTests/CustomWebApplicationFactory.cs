using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

/* EFCore In Memory Database Provider
 * 
 *          ---> Microsoft.EntityFrameworkCore.InMemory ---> Collection
 * DbContext
 *          ---> Microsoft.EntityFrameworkCore.SqlServer ---> SQL Server
 */

namespace CRUDTests;

// Create a custom web application factory that enable us to access the HTTP client that
// can send request to our web application.
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseEnvironment("IntegrationTest"); // notice this

        builder.ConfigureServices(services => 
        {
            // service descriptor represents the type of service and the service lifetime
            var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)); // to get our actual dbcontextoption service

            if (descriptor != null)
            {
                services.Remove(descriptor);    // remove our actual DbContextOption service for our actual database
            }

            services.AddDbContext<ApplicationDbContext>(options => 
            {
                options.UseInMemoryDatabase("DatabaseForTesting");      // Set dummy database for our Integration Test
            });
        });
    }
}
