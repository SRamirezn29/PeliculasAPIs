using System.ComponentModel.DataAnnotations;

namespace PeliculasAPIs.Entidades
{
    public class Genero : IId
    { 
        public int Id { get; set; }
        [Required]
        [StringLength(40)]
        public string Nombre { get; set; } = string.Empty;
        public List<PeliculasGenero> PeliculasGeneros { get; set; } = new();
    }
}
