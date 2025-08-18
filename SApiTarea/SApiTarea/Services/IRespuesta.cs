using SApiTarea.Models;

namespace SApiTarea.Services
{
    public interface IRespuesta
    {
        Respuestas RespuestaCorrecta(object contenido);
        Respuestas RespuestaIncorrecta(string mensaje);
    }
}
