using Microsoft.AspNetCore.Mvc;
using ViewComponentExample.Models;

namespace ViewComponentExample.ViewComponents;

public class GridViewComponent : ViewComponent
{
    // caught the argument by defining the method's parameter
    // The parameter name must has same name as the property's name
    // in anonymous object we define in the view when we pass it to InvokeAsync
    // in this case is 'model' in this parameter and 'model' in anonymous
    public async Task<IViewComponentResult> InvokeAsync(PersonGridModel model)
    {
        // Now we can continue to pass personGridModel we created in View, then pass it to
        // partial view

        //PersonGridModel model = new()
        //{
        //    Title = "Person List",
        //    Persons =
        //    {
        //        new Person() { Name = "John", JobTitle = "Manager" },
        //        new Person() { Name = "Jones", JobTitle = "Assistant Manager" },
        //        new Person() { Name = "William", JobTitle = "Clerk" },
        //    }
        //};

        //return View(model);

        return View(model);
    }
}