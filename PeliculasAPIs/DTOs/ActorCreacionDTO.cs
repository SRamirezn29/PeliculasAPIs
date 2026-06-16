using PeliculasAPIs.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPIs.DTOs
{
    public class ActorCreacionDTO
    {
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        [PesoArchivoValidaciones(PesoMaximoEnMegabytes: 4)]
        [TipoArchivoValidacion(grupoTipoArchivo: GrupoTipoArchivo.Imagen)]
        public required IFormFile Foto { get; set; }
    }
}
