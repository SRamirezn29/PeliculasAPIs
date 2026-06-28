using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeliculasAPIs.DTOs;
using PeliculasAPIs.Entidades;
using PeliculasAPIs.Helpers;
using PeliculasAPIs.Migrations;
using PeliculasAPIs.Servicios;

namespace PeliculasAPIs.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PelicuasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "peliculas";
        public PelicuasController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        public async Task<ActionResult<PeliculasIndexDTO>> Get()
        {
            var top = 5;
            var hoy = DateTime.Today;

            var proximoEstrenos = await context.Peliculas
                .Where(x => x.FechaEstreno > hoy)
                .OrderBy(x => x.FechaEstreno)
                .Take(top)
                .ToListAsync();


            var enCines = await context.Peliculas
                .Where(x => x.EnCines)
                .Take(top)
                .ToListAsync();

            var resultado = new PeliculasIndexDTO();
            resultado.FuturosEstrenos = mapper.Map<List<PeliculaDTO>>(proximoEstrenos);
            resultado.EnCines = mapper.Map<List<PeliculaDTO>>(enCines);
            return resultado;
        }

        [HttpGet("filtro")]
        public async Task<ActionResult<List<PeliculaDTO>>> Filtrar([FromQuery] FiltroPeliculaDTO filtroPeliculaDTO)
        {
            var peliculaQueryable = context.Peliculas.AsQueryable();

            if(!string.IsNullOrEmpty(filtroPeliculaDTO.Titulo))
            {
                peliculaQueryable = peliculaQueryable.Where(x => x.Titulo.Contains(filtroPeliculaDTO.Titulo));
            }

            if(filtroPeliculaDTO.EnCines)
            {
                peliculaQueryable = peliculaQueryable.Where(x => x.EnCines);
            }

            if(filtroPeliculaDTO.ProximosEstrenos)
            {
                var hoy = DateTime.Today;
                peliculaQueryable = peliculaQueryable.Where(x => x.FechaEstreno > hoy);
            }

            if( filtroPeliculaDTO.GeneroId != 0)
            {
                peliculaQueryable = peliculaQueryable
                    .Where(x => x.PeliculasGeneros.Select(y => y.GeneroId)
                    .Contains(filtroPeliculaDTO.GeneroId));
            }

            await HttpContext.InsertarParametrosPaginacion(peliculaQueryable,
                filtroPeliculaDTO.CantidadRegistroPorPagina);

            var peliculas = await peliculaQueryable.Paginar(filtroPeliculaDTO.Paginacion).ToListAsync();

            return mapper.Map<List<PeliculaDTO>>(peliculas);
        }



        [HttpGet("{id}", Name = "obtenerPelicula")]
        public async Task<ActionResult<PeliculaDTO>> Get(int id)
        {
            var pelicula = await context.Peliculas.FirstOrDefaultAsync(x => x.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }
            return mapper.Map<PeliculaDTO>(pelicula);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);

            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    pelicula.Poster = await almacenadorArchivos.GuardarArchivo(contenido, extension, contenedor,
                        peliculaCreacionDTO.Poster.ContentType);
                }
            }
            AsignarOrdenActores(pelicula);
            context.Add(pelicula);
            context.Add(pelicula);
            await context.SaveChangesAsync();
            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);
            return CreatedAtRoute("obtenerPelicula", new { id = pelicula.Id }, peliculaDTO);
        }

        private void AsignarOrdenActores(Pelicula pelicula)
        {
            if(pelicula.PeliculasActores != null)
            {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i;
                }
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var peliculaDB = await context.Peliculas
                .Include(x => x.PeliculasActores)
                .Include(x => x.PeliculasGeneros)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (peliculaDB == null)
            {
                return NotFound();
            }
            peliculaDB = mapper.Map(peliculaCreacionDTO, peliculaDB);
            if (peliculaCreacionDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await peliculaCreacionDTO.Poster.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(peliculaCreacionDTO.Poster.FileName);
                    peliculaDB.Poster = await almacenadorArchivos.EditarArchivo(contenido, extension, contenedor, peliculaDB.Poster,
                        peliculaCreacionDTO.Poster.ContentType);
                }
            }
            AsignarOrdenActores(peliculaDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<PeliculaPachDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }
            var entidadDB = await context.Peliculas.FirstOrDefaultAsync(a => a.Id == id);

            if (entidadDB == null)
            {
                return NotFound();
            }
            var entidadDTO = mapper.Map<PeliculaPachDTO>(entidadDB);
            patchDocument.ApplyTo(entidadDTO, ModelState);
            var isValid = TryValidateModel(entidadDTO);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }
            mapper.Map(entidadDTO, entidadDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Peliculas.AnyAsync(a => a.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            context.Remove(new Pelicula { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
