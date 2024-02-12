using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityManager.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    public string Get()             // this get invoked when request from browser go to this "/api/test/" path
    {                               // the method's name is must be "Get" for GET request
        return "Hello World";
    }

    [HttpGet]
    public string SomeMethodName()  // or if you want different name for the action method, you can apply [HttpGet] attribute
    {                               // on that named method like this
        return "Hello World!!!";
    }
}
