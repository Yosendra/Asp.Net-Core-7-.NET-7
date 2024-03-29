﻿using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace CRUDExample;

public static class ConfigureServicesExtension
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

    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<PersonListActionFilter>();
        services.AddTransient<ResponseHeaderActionFilter>();
        services.AddHttpLogging(configureOptions => configureOptions.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders);
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddRepositories();
        services.AddServices();
        services.AddControllersWithViews(options => 
        {
            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
            options.Filters.Add(new ResponseHeaderActionFilter(logger)
            {
                Key = "Key-From-Global",
                Value = "Value-From-Global",
                Order = 2,
            });
        });

        return services;
    }
}
