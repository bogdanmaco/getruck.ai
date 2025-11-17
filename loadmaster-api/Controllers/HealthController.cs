using Microsoft.AspNetCore.Mvc;

namespace loadmaster_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok(new { status = "ok" });
    }
}


