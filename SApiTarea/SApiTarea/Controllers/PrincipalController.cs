using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SApiTarea.Models;
using SApiTarea.Services;

namespace SApiTarea.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrincipalController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _environment;
        private readonly IRespuesta _respuesta;

        public PrincipalController(IConfiguration configuration, IHostEnvironment environment, IRespuesta respuesta)
        {
            _configuration = configuration;
            _environment = environment;
            _respuesta = respuesta;
        }

        [HttpGet]
        [Route("ConsultarCasas")]
        public IActionResult ConsultarCasas()
        {

            using (var contexto = new SqlConnection((_configuration.GetSection("ConnectionStrings:Connection").Value)))
            {
                //select
                var resultado = contexto.Query<Casa>("sp_ListarCasasDisponibles", new
                {
                });
                if (resultado.Any())
                {
                    var filtrar = resultado
                        .Where(c => c.PrecioCasa >= 115000 && c.PrecioCasa <= 180000)
                        .OrderByDescending(c => c.UsuarioAlquiler == null) 
                        .ToList();
                    return Ok(_respuesta.RespuestaCorrecta(filtrar));
                }
                else
                {
                    return BadRequest(_respuesta.RespuestaIncorrecta("Su info no fue encontrada."));
                }
            }

        }
    }
}