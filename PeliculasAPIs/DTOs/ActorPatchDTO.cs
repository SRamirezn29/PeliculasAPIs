using System.ComponentModel.DataAnnotations;

namespace PeliculasAPIs.DTOs
{
    public class ActorPatchDTO
    {
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
    }
}
