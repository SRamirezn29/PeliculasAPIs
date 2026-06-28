using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace PeliculasAPIs.Entidades
{
    public class Pelicula
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; } = string.Empty;
        public bool EnCines { get; set; }
        public DateTime FechaEstreno { get; set; }
        public string Poster { get; set; } = string.Empty;
        public List<PeliculasActores> PeliculasActores { get; set; }
        public List<PeliculasGenero> PeliculasGeneros { get; set; }
    }
}
