using AutoMapper;
using PeliculasAPIs.DTOs;
using PeliculasAPIs.Entidades;

namespace PeliculasAPIs.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>().ReverseMap();

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>().ForMember(a => a.Foto, opt => opt.Ignore());  
        }
    }
}
