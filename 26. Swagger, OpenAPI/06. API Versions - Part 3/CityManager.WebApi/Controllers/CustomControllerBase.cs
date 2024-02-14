using Microsoft.AspNetCore.Mvc;

namespace CityManager.WebApi.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class CustomControllerBase : ControllerBase
{
}
