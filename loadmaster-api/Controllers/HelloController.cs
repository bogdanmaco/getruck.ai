using Microsoft.AspNetCore.Mvc;

namespace loadmaster_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery] string? name)
        {
            var who = string.IsNullOrWhiteSpace(name) ? "world" : name;
            return Ok(new { message = $"Hello, {who}!" });
        }
    }
}


