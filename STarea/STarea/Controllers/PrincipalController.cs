using Microsoft.AspNetCore.Mvc;
using STarea.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using static System.Net.WebRequestMethods;

namespace STarea.Controllers
{
    public class PrincipalController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _http;

        public PrincipalController(IConfiguration configuration, IHttpClientFactory http)
        {
            _configuration = configuration;
            _http = http;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConsultarCasas()
        {
            using (var http = _http.CreateClient())
            {

                http.BaseAddress = new Uri(_configuration.GetSection("Start:ApiUrl").Value!);

                var resultado = http.GetAsync("api/Principal/ConsultarCasas").Result;

                if (resultado.IsSuccessStatusCode)
                {
                    var datos = resultado.Content.ReadFromJsonAsync<Respuesta<List<Casa>>>().Result;



                    return View(datos?.Contenido);
                }
                else
                {
                    var respuesta = resultado.Content.ReadFromJsonAsync<Respuesta>().Result;
                    ViewBag.Mensaje = respuesta?.Mensaje;
                    return View(new List<Casa>());
                }
            }
        }
    }
}