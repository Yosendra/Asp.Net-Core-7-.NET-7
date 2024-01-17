using ServiceContracts;
using Services;

namespace CRUDExample.Bootstrap;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ICountryService, CountryService>()
            .AddScoped<IPersonService, PersonService>();
    }
}
