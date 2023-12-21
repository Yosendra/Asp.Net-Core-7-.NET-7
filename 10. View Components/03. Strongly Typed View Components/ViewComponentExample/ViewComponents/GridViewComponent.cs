using Microsoft.AspNetCore.Mvc;
using ViewComponentExample.Models;

namespace ViewComponentExample.ViewComponents;

public class GridViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        PersonGridModel model = new()
        {
            GridTitle = "Person List",
            Persons =
            {
                new Person() { Name = "John", JobTitle = "Manager" },
                new Person() { Name = "Jones", JobTitle = "Assistant Manager" },
                new Person() { Name = "William", JobTitle = "Clerk" },
            }
        };
        // Now, instead of passing the model to ViewData, we pass model to the view imediately
        // using Strongly Typed View Components mechanism
        //ViewData["Grid"] = model;

        return View(model);
    }
}