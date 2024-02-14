using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityManager.WebApi.Controllers;

public class TestController : CustomControllerBase
{
    [HttpGet]
    public string SomeMethodName()
    {
        return "Hello World!!!";
    }
}
