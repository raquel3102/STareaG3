using SApiTarea.Models;

namespace SApiTarea.Services
{
    public class Respuesta : IRespuesta
    {
        private readonly IConfiguration _configuration;

        public Respuesta(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Respuestas RespuestaCorrecta(object contenido)
        {
            return new Respuestas
            {
                Codigo = 0,
                Mensaje = "Operación exitosa",
                Contenido = contenido

            };
        }

        public Respuestas RespuestaIncorrecta(string mensaje)
        {
            return new Respuestas
            {
                Codigo = 99,
                Mensaje = mensaje,
                Contenido = null

            };
        }
    }
}
