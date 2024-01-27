using Microsoft.AspNetCore.Mvc.Filters;

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
    }
}
