namespace STarea.Models
{
    public class Respuesta
    {
        public int Codigo { get; set; }
        public string? Mensaje { get; set; }
        public object? Contenido { get; set; }
    }

    public class Respuesta<T>
    {
        public int Codigo { get; set; }
        public string? Mensaje { get; set; }
        public T? Contenido { get; set; }
    }
}
