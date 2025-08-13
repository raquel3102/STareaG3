using Microsoft.AspNetCore.Mvc;
using SApiTarea.Models;
using SApiTarea.Services;

namespace SApiTarea.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AbonoController : ControllerBase
    {
        private readonly DapperService _service;

        public AbonoController(DapperService service)
        {
            _service = service;
        }

        [HttpGet("saldo/{idCompra}")]
        public async Task<IActionResult> ObtenerSaldo(long idCompra)
        {
            var saldo = await _service.ObtenerSaldoAsync(idCompra);
            return Ok(new { success = true, saldo });
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarAbono([FromBody] AbonoModel model)
        {
            var success = await _service.RegistrarAbonoAsync(model);
            return Ok(new { success });
        }
    }
}