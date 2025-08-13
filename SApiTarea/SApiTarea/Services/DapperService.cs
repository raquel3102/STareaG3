using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using SApiTarea.Models;

namespace SApiTarea.Services
{
    public class DapperService
    {
        private readonly string _connectionString;

        public DapperService(string? connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<IEnumerable<ConsultaModel>> GetProductosPorEstadoAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.QueryAsync<ConsultaModel>(
                "ProductosPorEstado", commandType: CommandType.StoredProcedure);
            return result ?? Enumerable.Empty<ConsultaModel>();
        }

        public async Task<IEnumerable<ConsultaModel>> GetComprasPendientesAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.QueryAsync<ConsultaModel>(
                "CargarComprasPendientes", commandType: CommandType.StoredProcedure);
            return result ?? Enumerable.Empty<ConsultaModel>();
        }

        public async Task<decimal> ObtenerSaldoAsync(long idCompra)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.QueryFirstOrDefaultAsync<decimal?>(
                "SELECT Saldo FROM Principal WHERE Id_Compra = @Id", new { Id = idCompra });
            return result ?? 0m;
        }

        public async Task<bool> RegistrarAbonoAsync(AbonoModel model)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            //Ejecutar el procedimiento con los nombres correctos
            var parametros = new DynamicParameters();
            parametros.Add("@IdCompra", model.Id_Compra);
            parametros.Add("@MontoAbono", model.Monto);

            await connection.ExecuteAsync(
                "RegistrarAbono",
                parametros,
                commandType: CommandType.StoredProcedure);

            //Verificar si el saldo quedó en cero
            var compra = await connection.QueryFirstOrDefaultAsync<ConsultaModel>(
                "SELECT Saldo FROM Principal WHERE Id_Compra = @IdCompra",
                new { IdCompra = model.Id_Compra });

            if (compra != null && Math.Round(compra.Saldo, 2) == 0)
            {
                await connection.ExecuteAsync(
                    "UPDATE Principal SET Estado = 'Cancelado' WHERE Id_Compra = @IdCompra",
                    new { IdCompra = model.Id_Compra });
            }

            return true;
        }
    }
}