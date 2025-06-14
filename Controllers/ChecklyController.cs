using Microsoft.AspNetCore.Mvc;

namespace Interasian.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklyController : ControllerBase
    {

        public ChecklyController()
        {

        }

        [HttpGet]
        public IActionResult Checkly()
        {
            return Ok("Keep me running Boi");
        }
    }
} 