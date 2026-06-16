using System.ComponentModel.DataAnnotations;

namespace PeliculasAPIs.DTOs
{
    public class GeneroCreacionDTO
    {
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        [StringLength(40)]
        public string Nombre { get; set; } = string.Empty;
    }
}
