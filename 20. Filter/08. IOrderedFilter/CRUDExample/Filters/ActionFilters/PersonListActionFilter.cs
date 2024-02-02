using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Filters.ActionFilters;

public class PersonListActionFilter : IActionFilter
{
    private readonly ILogger<PersonListActionFilter> _logger;

    public PersonListActionFilter(ILogger<PersonListActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("{FilterName}.{MethodName} method",
            nameof(PersonListActionFilter),
            nameof(OnActionExecuting));

        // This is needed in order to access the action arguments at OnActionExecuted()
        context.HttpContext.Items["arguments"] = context.ActionArguments;

        if (context.ActionArguments.ContainsKey("searchBy"))
        {
            string? searchBy = Convert.ToString(context.ActionArguments["searchBy"]);
            if (!string.IsNullOrEmpty(searchBy))
            {
                List<string> searchByOptions = new()
                {
                    nameof(PersonResponse.Name),
                    nameof(PersonResponse.Email),
                    nameof(PersonResponse.DateOfBirth),
                    nameof(PersonResponse.Gender),
                    nameof(PersonResponse.CountryId),
                    nameof(PersonResponse.Address),
                };

                if (!searchByOptions.Any(option => option == searchBy))
                {
                    const string MessageTemplate = "searchBy actual value is {searchBy}";
                    _logger.LogInformation(MessageTemplate, searchBy);
                    context.ActionArguments["searchBy"] = nameof(PersonResponse.Name);
                    _logger.LogInformation(MessageTemplate, context.ActionArguments["searchBy"]);
                }
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("{FilterName}.{MethodName} method",
            nameof(PersonListActionFilter),
            nameof(OnActionExecuted));

        var controller = (PersonController) context.Controller;

        // Accessing the action arguments we have stored in OnActionExecuting() before
        IDictionary<string, object?>? arguments = (IDictionary<string, object?>?) context.HttpContext.Items["arguments"];

        if (arguments != null)
        {
            if (arguments.ContainsKey("searchBy"))
                controller.ViewData["CurrentSearchBy"] = arguments["searchBy"];

            if (arguments.ContainsKey("keyword"))
                controller.ViewData["CurrentKeyword"] = arguments["keyword"];

            if (arguments.ContainsKey("sortBy"))
                controller.ViewData["CurrentSortBy"] = arguments["sortBy"];

            if (arguments.ContainsKey("sortOrder"))
                controller.ViewData["CurrentSortOrder"] = ((SortOrderEnum)arguments["sortOrder"]!).ToString();
        }

        controller.ViewBag.SearchFields = new Dictionary<string, string>()
        {
            {nameof(PersonResponse.Name), "Name"},
            {nameof(PersonResponse.Email), "Email"},
            {nameof(PersonResponse.DateOfBirth), "Date of Birth"},
            {nameof(PersonResponse.Gender), "Gender"},
            {nameof(PersonResponse.CountryId), "Country"},
            {nameof(PersonResponse.Address), "Address"},
        };
    }
}
