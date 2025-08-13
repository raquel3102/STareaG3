namespace SApiTarea.Models
{
    public class ConsultaModel
    {
        public long Id_Compra { get; set; }
        public decimal Precio { get; set; }
        public decimal Saldo { get; set; }
        public string? Descripcion { get; set; }
        public string? Estado { get; set; }
    }
}