using Microsoft.AspNetCore.Mvc;
using SApiTarea.Services;

namespace SApiTarea.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrincipalController : ControllerBase
    {
        private readonly DapperService _service;

        public PrincipalController(DapperService service)
        {
            _service = service;
        }

        [HttpGet("estado")]
        public async Task<IActionResult> GetProductosPorEstado()
        {
            var productos = await _service.GetProductosPorEstadoAsync();
            return Ok(productos);
        }

        [HttpGet("pendientes")]
        public async Task<IActionResult> GetComprasPendientes()
        {
            var compras = await _service.GetComprasPendientesAsync();
            return Ok(compras);
        }
    }
}