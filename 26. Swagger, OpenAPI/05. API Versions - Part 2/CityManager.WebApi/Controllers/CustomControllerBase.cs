using Microsoft.AspNetCore.Mvc;

namespace CityManager.WebApi.Controllers;

//[Route("api/v{version:apiVersion}/[controller]")]
[Route("api/[controller]")]  // When we are not using "UrlSegmentApiVersionReader" for API reader
[ApiController]
public class CustomControllerBase : ControllerBase
{
}
