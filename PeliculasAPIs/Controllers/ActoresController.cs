using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPIs.DTOs;
using PeliculasAPIs.Entidades;
using PeliculasAPIs.Servicios;

namespace PeliculasAPIs.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "actores";
        public ActoresController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActorDTO>>> Get()
        {
            var entidades = await context.Actores.ToListAsync();
            return mapper.Map<List<ActorDTO>>(entidades);
        }


        [HttpGet("{id}", Name = "obtenerActor")]
        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var entidad = await context.Actores.FirstOrDefaultAsync(a => a.Id == id);
            if (entidad == null)
            {
                return NotFound();
            }
            return mapper.Map<ActorDTO>(entidad);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var entidad = mapper.Map<Actor>(actorCreacionDTO);
            if(actorCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    entidad.Foto= await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor,
                        actorCreacionDTO.Foto.ContentType);
                }
            }
            context.Add(entidad);
            await context.SaveChangesAsync();
            var dto = mapper.Map<ActorDTO>(entidad);
            return new CreatedAtRouteResult("obtenerActor", new { id = entidad.Id }, dto);
        }

        [HttpPut("Id")]
        public async Task<ActionResult> Put(int id, [FromBody] ActorCreacionDTO actorCreacionDTO)
        {
            var actorDB = await context.Actores.FirstOrDefaultAsync(a => a.Id == id);
            if (actorDB == null)
            {
                return NotFound();
            }
            actorDB = mapper.Map(actorCreacionDTO, actorDB);
            if (actorCreacionDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreacionDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreacionDTO.Foto.FileName);
                    actorDB.Foto = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor, ruta: actorDB.Foto,
                        actorCreacionDTO.Foto.ContentType);
                }
            }
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Actores.AnyAsync(a => a.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Actor { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
