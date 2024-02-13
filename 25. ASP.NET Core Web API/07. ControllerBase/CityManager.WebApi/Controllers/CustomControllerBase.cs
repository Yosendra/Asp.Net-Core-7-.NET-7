using Microsoft.AspNetCore.Mvc;

namespace CityManager.WebApi.Controllers;

[Route("api/[controller]")]     // notice this
[ApiController]                 // put these attribute here instead
public class CustomControllerBase : ControllerBase
{
}