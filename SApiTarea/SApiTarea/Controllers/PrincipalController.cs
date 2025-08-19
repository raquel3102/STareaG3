using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SApiTarea.Models;
using SApiTarea.Services;
using System.Data;

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

        [HttpPost]
        [Route("Alquilar")]
        public IActionResult Alquilar(Casa casa)
        {
            if (casa == null || string.IsNullOrWhiteSpace(casa.UsuarioAlquiler))
            {
                return BadRequest("Debe ingresar el usuario que alquila y la casa.");
            }

            using (var connection = new SqlConnection((_configuration.GetSection("ConnectionStrings:Connection").Value)))
            {
                var resultado = connection.Execute("sp_AlquilarCasa",
                    new
                    {
                        IdCasa = casa.IdCasa,
                        UsuarioAlquiler = casa.UsuarioAlquiler
                    }
                );

                if (resultado > 0)
                {
                    return Ok(_respuesta.RespuestaCorrecta(casa));
                }
                else
                {
                    return BadRequest("No se pudo realizar el alquiler. Verifique la información.");
                }
            }
        }
    }
}