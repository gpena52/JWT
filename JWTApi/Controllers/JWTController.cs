using Microsoft.AspNetCore.Mvc;

namespace JWTApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JWTController : ControllerBase
    {
        private readonly ILogger<JWTController> _logger;

        public JWTController(ILogger<JWTController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
