using System.ComponentModel.DataAnnotations;

namespace PeliculasAPIs.DTOs
{
    public class PeliculaPachDTO
    {
        [Required]
        [StringLength(300)]
        public string Titulo { get; set; } = string.Empty;
        public bool EnCines { get; set; }
        public DateTime FechaEstreno { get; set; }
    }
}
