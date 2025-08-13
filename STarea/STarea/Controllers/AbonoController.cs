using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using STarea.Models;
using STarea.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STarea.Controllers
{
    public class AbonoController : Controller
    {
        private readonly ApiService _apiService;

        public AbonoController(ApiService apiService)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
        }

        [HttpGet]
        public async Task<IActionResult> Registro()
        {
            try
            {
                await CargarComprasPendientesAsync();
                return View(new AbonoModel()); // Asegura que el modelo no sea nulo
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Registro(AbonoModel model)
        {
            try
            {
                await CargarComprasPendientesAsync();

                if (!ModelState.IsValid)
                {
                    ViewBag.Mensaje = "Datos inválidos. Verifique los campos.";
                    return View(model);
                }

                var saldoActual = await _apiService.ObtenerSaldoAsync(model.Id_Compra);

                if (saldoActual == 0)
                {
                    ViewBag.Mensaje = "La compra no existe o el saldo ya está en cero.";
                    return View(model);
                }

                if (model.Monto > saldoActual)
                {
                    ViewBag.Mensaje = "El monto del abono no puede ser mayor que el saldo actual.";
                    return View(model);
                }

                var resultado = await _apiService.RegistrarAbonoAsync(model);

                if (!resultado)
                {
                    ViewBag.Mensaje = "Error al registrar el abono.";
                    return View(model);
                }

                TempData["MensajeExito"] = "Abono registrado correctamente.";
                return RedirectToAction("Consulta", "Principal");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = $"Ocurrió un error al registrar el abono: {ex.Message}";
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerSaldo(long idCompra)
        {
            try
            {
                var saldo = await _apiService.ObtenerSaldoAsync(idCompra);

                if (saldo >= 0)
                {
                    return Json(new { success = true, saldo });
                }

                return Json(new { success = false, message = "Compra no encontrada" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al obtener saldo: {ex.Message}" });
            }
        }

        private async Task CargarComprasPendientesAsync()
        {
            var info = await _apiService.GetComprasPendientesAsync();

            var pendienteCombo = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Seleccione" }
            };

            foreach (var item in info)
            {
                pendienteCombo.Add(new SelectListItem
                {
                    Value = item.Id_Compra.ToString(),
                    Text = item.Descripcion
                });
            }

            ViewBag.PuestoCombo = pendienteCombo;
        }
    }
}