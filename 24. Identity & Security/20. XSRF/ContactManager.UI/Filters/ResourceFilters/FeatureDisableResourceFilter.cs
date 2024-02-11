using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResourceFilters;

public class FeatureDisableResourceFilter : IAsyncResourceFilter
{
    private readonly ILogger<FeatureDisableResourceFilter> _logger;
    private readonly bool _isDisabled;

    public FeatureDisableResourceFilter(ILogger<FeatureDisableResourceFilter> logger, bool isDisabled = true)
    {
        _logger = logger;
        _isDisabled = isDisabled;
    }

    public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
    {
        // BEFORE
        _logger.LogInformation("{FilterName}.{MethodName} - BEFORE", 
            nameof(FeatureDisableResourceFilter), 
            nameof(OnResourceExecutionAsync));

        if (_isDisabled)
        {
            context.Result = new StatusCodeResult(501);     // 501 - not implemented yet
        }
        else
        {
            await next();

            // AFTER
            _logger.LogInformation("{FilterName}.{MethodName} - AFTER",
                nameof(FeatureDisableResourceFilter),
                nameof(OnResourceExecutionAsync));
        }
    }
}
