using Microsoft.AspNetCore.Mvc;
using STarea.Services;
using STarea.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace STarea.Controllers
{
    public class PrincipalController : Controller
    {
        private readonly ApiService _apiService;

        public PrincipalController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Consulta()
        {
            try
            {
                List<ConsultaModel> info = await _apiService.GetConsultasAsync();
                return View(info);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
    }
}