using Microsoft.AspNetCore.Mvc;
using PeliculasAPIs.Helpers;
using PeliculasAPIs.Validaciones;

namespace PeliculasAPIs.DTOs
{
    public class PeliculaCreacionDTO : PeliculaPachDTO
    {

        [PesoArchivoValidaciones(PesoMaximoEnMegabytes: 4)]
        [TipoArchivoValidacion(GrupoTipoArchivo.Imagen)]
        public IFormFile Poster { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenerosIDs { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorPeliculasCreacionDTO>>))]
        public List<ActorPeliculasCreacionDTO> Actores { get; set; } = new();
    }
}
