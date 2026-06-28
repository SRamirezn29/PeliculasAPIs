namespace PeliculasAPIs.DTOs
{
    public class FiltroPeliculaDTO
    {
        public int Pagina { get; set; }
        public int CantidadRegistroPorPagina { get; set; } = 10;
        public PaginacionDto Paginacion
        {
            get { return new PaginacionDto() { Pagina = Pagina, CantidadRegistrosPorPagina = CantidadRegistroPorPagina}; }
        }

        public string Titulo { get; set; }
        public int GeneroId { get; set; }
        public bool EnCines { get; set; }
        public bool ProximosEstrenos { get; set; }
    }
}
