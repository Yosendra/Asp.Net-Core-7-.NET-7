using Repositories;
using RepositoryContracts;
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

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<ICountryRepository, CountryRepository>()
            .AddScoped<IPersonRepository, PersonRepository>();
    }
}
