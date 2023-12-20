using Microsoft.AspNetCore.Mvc;

namespace ViewComponentExample.ViewComponents;

// Should be be suffixed with 'ViewComponent' or use attribut [ViewComponent]
//[ViewComponent]
public class GridViewComponent : ViewComponent  
{
    // The method name is fixed in this View Component class
    // Automatically get invoked when we invoke the View Component at View
    public async Task<IViewComponentResult> InvokeAsync()
    {
        // This is not regular view but partial view
        // The default location of the view component is at 'Views/Shared/Components/[component_name]/Default.cshtml'
        // So it will render the file at 'Views/Shared/Components/Grid/Default.cshtml' (without the suffix)
        // If you name it other than 'Default.cshtml', then you have to explicitly pass the name at 'return View();' as argument
        //return View("Sample");
        return View();
    }
}
