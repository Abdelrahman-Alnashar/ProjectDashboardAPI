using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProjectDashboardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecureController : ControllerBase
    {
        [HttpGet("secret")]
        [Authorize]
        public IActionResult SecretData()
        {
            return Ok("THis is secured data, you are logged in");
        }
    }
}
