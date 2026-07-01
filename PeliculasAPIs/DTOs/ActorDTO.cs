using System.ComponentModel.DataAnnotations;

namespace PeliculasAPIs.DTOs
{
    public class ActorDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; }
        public string Foto { get; set; } = string.Empty;
    }
}
