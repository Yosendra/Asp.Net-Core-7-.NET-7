using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

namespace CRUDExample.Filters.ActionFilters;

public class PersonListActionFilter : IActionFilter
{
    private readonly ILogger<PersonListActionFilter> _logger;

    public PersonListActionFilter(ILogger<PersonListActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("PersonListActionFilter.OnActionExecuted method");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("PersonListActionFilter.OnActionExecuting method");

        if (context.ActionArguments.ContainsKey("searchBy"))
        {
            string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);

            // validate searchBy value
            if (!string.IsNullOrEmpty(searchBy))
            {
                List<string> searchByOptions = new ()
                {
                    nameof(PersonResponse.Name),
                    nameof(PersonResponse.Email),
                    nameof(PersonResponse.DateOfBirth),
                    nameof(PersonResponse.Gender),
                    nameof(PersonResponse.CountryId),
                    nameof(PersonResponse.Address),
                };

                // reset the searchBy argument value
                // If the user provide any searchBy beside the available options, we will assign searchBy to "Name"
                if (!searchByOptions.Any(option => option == searchBy))
                {
                    const string MessageTemplate = "searchBy actual value is {0}";

                    _logger.LogInformation(MessageTemplate, searchBy);

                    context.ActionArguments["searchBy"] = nameof(PersonResponse.Name);

                    _logger.LogInformation(MessageTemplate, context.ActionArguments["searchBy"]);
                }
            }
        }
    }
}
