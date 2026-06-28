using System.ComponentModel.DataAnnotations;

namespace PeliculasAPIs.DTOs
{
    public class PeliculaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public bool EnCines { get; set; }
        public DateTime FechaEstreno { get; set; }
        public string Poster { get; set; } = string.Empty;
    }
}
