using Microsoft.AspNetCore.Mvc;

namespace CityManager.WebApi.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]          // add version to the route
[ApiController]
public class CustomControllerBase : ControllerBase
{
}
