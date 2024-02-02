using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;

namespace CRUDExample.Filters.ActionFilters;

public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
{
    private readonly ICountryService _countryService;

    public PersonCreateAndEditPostActionFilter(ICountryService countryService)
    {
        _countryService = countryService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // BEFORE
        if (context.Controller is PersonController controller)
        {
            if (!controller.ModelState.IsValid)
            {
                List<CountryResponse> countries = await _countryService.GetAllCountries();
                controller.ViewBag.Countries = countries.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
                controller.ViewBag.Errors = controller.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                // Notice this, to get the argument value in the action
                var requestModel = context.ActionArguments["requestModel"];

                // As soon you assign non-null value to the Result, it considered we are short-circuiting the action method
                // It means the subsequent filter, as well as the actual action method will not be executed
                context.Result = controller.View(requestModel);

                // Because we are short-circuiting, we should not invoke the "next" delegate
            }
            else
            {
                await next();
            }
        }
        else
        {
            await next();
        }

        // AFTER
    }
}
