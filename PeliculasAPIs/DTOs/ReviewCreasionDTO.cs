using System.ComponentModel.DataAnnotations;

namespace PeliculasAPIs.DTOs
{
    public class ReviewCreasionDTO
    {
        public string Comentario { get; set; }
        [Range(1, 5)]
        public int Puntuacion { get; set; }
    }
}
