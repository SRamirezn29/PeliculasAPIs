using AutoMapper;
using Microsoft.Extensions.Options;
using PeliculasAPIs.DTOs;
using PeliculasAPIs.Entidades;

namespace PeliculasAPIs.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>().ReverseMap();

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>().ForMember(a => a.Foto, options => options.Ignore());
            CreateMap<ActorPatchDTO, Actor>().ReverseMap();

            CreateMap<Pelicula, PeliculaDTO>().ReverseMap();
            CreateMap<PeliculaCreacionDTO, Pelicula>()
                .ForMember(p => p.Poster, options => options.Ignore())
                .ForMember(p => p.PeliculasGeneros, options => options.MapFrom(MapPeliculasGeneros))
                .ForMember(p => p.PeliculasActores, options => options.MapFrom());



            CreateMap<PeliculaPachDTO, Pelicula>().ReverseMap();
        }

        private List<PeliculasGenero> MapPeliculasGeneros(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<PeliculasGenero>();
            if (peliculaCreacionDTO.GenerosIDs == null) { return resultado; }
            foreach (var id in peliculaCreacionDTO.GenerosIDs)
            {
                resultado.Add(new PeliculasGenero() { GeneroId = id });
            }

            return resultado;
        }

        private List<PeliculasActores> MapPeliculasActores(PeliculaCreacionDTO peliculaCreacionDTO, Pelicula pelicula)
        {
            var resultado = new List<PeliculasActores>();
            if (peliculaCreacionDTO.Actores == null) { return resultado; }
            foreach (var actor in peliculaCreacionDTO.Actores)
            {
                resultado.Add(new PeliculasActores() { ActorId = actor.ActorId, Personaje = actor.Personaje });
            }
            return resultado;
        }
    }
}
