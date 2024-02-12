using Api.Versioning.Notification.Bug.Api.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Api.Versioning.Notification.Bug.Api.Controllers.V0_9;

[V0_9]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}