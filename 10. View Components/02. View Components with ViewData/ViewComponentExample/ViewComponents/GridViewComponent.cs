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
        // This ViewData only be shared on ViewComponent
        // This ViewData is not the same with the ViewData in the View
        ViewData["Grid"] = model;

        return View();
    }
}
