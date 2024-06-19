using Blog.API.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[ApiController]
[Route("")]
public class HomeController : ControllerBase
{
    [HttpGet("")]
    [ApiKey]
    public IActionResult Get()
    {
        return Ok();
    }
}
