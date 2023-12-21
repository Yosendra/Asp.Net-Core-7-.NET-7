using Microsoft.AspNetCore.Mvc;
using ViewComponentExample.Models;

namespace ViewComponentExample.ViewComponents;

public class GridViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(PersonGridModel model)
    {
        model ??= new();

        return View(model);
    }
}