using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using STarea.Models;

namespace STarea.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;

        public ApiService(HttpClient client)
        {
            _client = client;
        }

        // Obtener saldo por Id_Compra
        public async Task<decimal> ObtenerSaldoAsync(long idCompra)
        {
            var response = await _client.GetAsync($"api/abono/saldo/{idCompra}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<SaldoResponse>();
                return result?.Saldo ?? -1;
            }
            return -1;
        }

        // Registrar un abono
        public async Task<bool> RegistrarAbonoAsync(AbonoModel model)
        {
            var response = await _client.PostAsJsonAsync("api/abono", model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GenericResponse>();
                return result?.Success ?? false;
            }
            return false;
        }

        // Obtener compras pendientes
        public async Task<List<ConsultaModel>> GetComprasPendientesAsync()
        {
            var result = await _client.GetFromJsonAsync<List<ConsultaModel>>("api/principal/pendientes");
            return result ?? new List<ConsultaModel>();
        }

        // Obtener todas las consultas (productos por estado)
        public async Task<List<ConsultaModel>> GetConsultasAsync()
        {
            var result = await _client.GetFromJsonAsync<List<ConsultaModel>>("api/principal/estado");
            return result ?? new List<ConsultaModel>();
        }
    }

    // Modelos auxiliares para respuestas JSON
    public class SaldoResponse
    {
        public bool Success { get; set; }
        public decimal Saldo { get; set; }
    }

    public class GenericResponse
    {
        public bool Success { get; set; }
    }
}