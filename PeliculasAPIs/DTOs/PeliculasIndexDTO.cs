namespace PeliculasAPIs.DTOs
{
    public class PeliculasIndexDTO
    {
        public List<PeliculaDTO> FuturosEstrenos { get; set; } = new();
        public List<PeliculaDTO> EnCines { get; set; } = new();
    }
}
