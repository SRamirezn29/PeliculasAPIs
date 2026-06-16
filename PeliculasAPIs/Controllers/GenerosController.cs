using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPIs.DTOs;
using PeliculasAPIs.Entidades;

namespace PeliculasAPIs.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public GenerosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GeneroDTO>>> Get()
        {
            var entidades = await context.Generos.ToListAsync();
            var dtos = mapper.Map<List<GeneroDTO>>(entidades);
            return dtos;
        }

        [HttpGet("{id:int}", Name = "ObtenerGenero")]
        public async Task<ActionResult<GeneroDTO>> Get(int id)
        {
            var entidad = await context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }
            var dto = mapper.Map<GeneroDTO>(entidad);
            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<GeneroCreacionDTO>> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var entidad = mapper.Map<Genero>(generoCreacionDTO);
            context.Add(entidad);
            await context.SaveChangesAsync();
            var generoDTO = mapper.Map<GeneroDTO>(entidad);

            return new CreatedAtRouteResult("ObtenerGenero", new { id = generoDTO.Id }, generoDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var entidad = mapper.Map<Genero>(generoCreacionDTO);
            entidad.Id = id;
            context.Entry(entidad).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete (int id)
        {
            var existe = await context.Generos.AnyAsync(x => x.Id == id);
            if(!existe)
            {
                return NoContent();
            }
            context.Remove(new Genero() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
