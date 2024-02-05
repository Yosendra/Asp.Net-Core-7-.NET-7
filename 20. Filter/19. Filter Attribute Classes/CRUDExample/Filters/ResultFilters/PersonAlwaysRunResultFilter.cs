using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResultFilters;

public class PersonAlwaysRunResultFilter : IAlwaysRunResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        // Skip if filter contains SkipFilter
        if (context.Filters.OfType<SkipFilter>().Any())
        {
            return;
        }
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        
    }
}
