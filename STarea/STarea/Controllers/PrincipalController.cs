using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using STarea.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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



        [HttpGet]
        public IActionResult Alquilar()
        {
            using (var http = _http.CreateClient())
            {
                http.BaseAddress = new Uri(_configuration.GetSection("Start:ApiUrl").Value!);

                // Llamamos al API que lista las casas
                var resultado = http.GetAsync("api/Principal/ConsultarCasas").Result;

                if (resultado.IsSuccessStatusCode)
                {
                    var casas = resultado.Content.ReadFromJsonAsync<Respuesta<List<Casa>>>().Result;

                    // Sólo mostrar las casas que NO están alquiladas
                    var casasDisponibles = casas!.Contenido!.Where(c => string.IsNullOrEmpty(c.UsuarioAlquiler)).ToList();

                    ViewBag.Casas = new SelectList(casasDisponibles, "IdCasa", "IdCasa");
                }
                else
                {
                    ViewBag.Casas = new SelectList(new List<Casa>(), "IdCasa", "IdCasa");
                }
            }

            return View();
        }


        [HttpPost]
        public IActionResult Alquilar (Casa casa)
        {
            using (var http = _http.CreateClient())
            {
                http.BaseAddress = new Uri(_configuration.GetSection("Start:ApiUrl").Value!);
                var resultado = http.PostAsJsonAsync("api/Principal/Alquilar", casa).Result;
                if (resultado.IsSuccessStatusCode)
                {
                    return RedirectToAction("ConsultarCasas", "Principal");
                }
                else
                {
                    var respuesta = resultado.Content.ReadFromJsonAsync<Respuesta>().Result;
                    ViewBag.Mensaje = respuesta!.Mensaje;
                    return View(casa);
                }
            }
        }



    }
}