using Microsoft.AspNetCore.Mvc;

namespace ConexaoSolidaria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            message = "Conexao Solidaria API funcionando"
        });
    }
}