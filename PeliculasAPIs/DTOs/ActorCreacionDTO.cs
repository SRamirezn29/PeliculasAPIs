using PeliculasAPIs.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPIs.DTOs
{
    public class ActorCreacionDTO : ActorPatchDTO
    {
        [PesoArchivoValidaciones(PesoMaximoEnMegabytes: 4)]
        [TipoArchivoValidacion(grupoTipoArchivo: GrupoTipoArchivo.Imagen)]
        public required IFormFile Foto { get; set; }
    }
}
