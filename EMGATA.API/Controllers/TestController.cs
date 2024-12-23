using Microsoft.AspNetCore.Mvc;

namespace EMGATA.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
	[HttpGet]
	public IActionResult Get()
	{
		return Ok(new { message = "API is working!" });
	}
}

// Dans un de vos contrôleurs
[ApiController]
[Route("api/[controller]")]
public class DebugController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public DebugController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("jwt-config")]
    public IActionResult GetJwtConfig()
    {
        return Ok(new
        {
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            KeyLength = _configuration["Jwt:Key"]?.Length ?? 0
        });
    }
}