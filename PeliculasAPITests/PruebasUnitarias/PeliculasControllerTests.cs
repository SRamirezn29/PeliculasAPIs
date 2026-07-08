using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NetTopologySuite.Utilities;
using PeliculasAPIs.Controllers;
using PeliculasAPIs.DTOs;
using PeliculasAPIs.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace PeliculasAPITests.PruebasUnitarias
{
    [TestClass]
    public class PeliculasControllerTests : BasePruebas
    {
        private string CrearDataPrueba()
        {
            var databaseName = Guid.NewGuid().ToString();
            var context = ConstruitContext(databaseName);
            var genero = new Genero() { Nombre = "genre 1" };

            var peliculas = new List<Pelicula>()
        {
            new Pelicula() { Titulo = "Pelicula 1", FechaEstreno = new DateTime(2010, 1,1), EnCines = false },
            new Pelicula() { Titulo = "No Estrenada ", FechaEstreno = DateTime.Today.AddDays(1),EnCines = false },
            new Pelicula() { Titulo = "Pelicula en Cines", FechaEstreno = DateTime.Today.AddDays(-1),EnCines = true }
        };

            var peliculaConGenero = new Pelicula()
            {
                Titulo = "Pelicula con Genero",
                FechaEstreno = new DateTime(2010, 1,1),
                EnCines = false
            };

            peliculas.Add(peliculaConGenero);

            context.Add(genero);
            context.AddRange(peliculas);
            context.SaveChanges();

            var peliculaGenero = new PeliculasGenero() { GeneroId = genero.Id , PeliculaId = peliculaConGenero.Id};
            context.Add(peliculaGenero);
            context.SaveChanges();

            return databaseName;
        }

        [TestMethod]
        public async Task FiltralPorTitulo()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruitContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            var controller = new PelicuasController(contexto, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var tituloPelicula = "Pelicula 1";
            var filtro = new FiltroPeliculaDTO() 
            { 
                Titulo = tituloPelicula,
                CantidadRegistroPorPagina = 10,
            };
            
            var resultado = await controller.Filtrar(filtroDTO);
            var peliculas = resultado.Value;
            Assert.AreEqual(1, peliculas.Count);
            Assert.AreEqual(tituloPelicula, peliculas[0].Titulo);
        }

        [TestMethod]
        public async Task FiltroEnCines()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruitContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            var controller = new PelicuasController(contexto, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filtroDTO = new FiltroPeliculaDTO()
            {
                EnCines = true,
            };

            var resultado = await controller.Filtrar(filtroDTO);
            var peliculas = resultado.Value;
            Assert.AreEqual(1, peliculas.Count);
            Assert.AreEqual("Pelicula en Cines", peliculas[0].Titulo);
        }

        [TestMethod]
        public async Task FiltrarProximosEstrenos()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruitContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            var controller = new PelicuasController(contexto, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filtroDTO = new FiltroPeliculaDTO()
            {
                ProximosEstrenos = true,
            };

            var resultado = await controller.Filtrar(filtroDTO);
            var peliculas = resultado.Value;
            Assert.AreEqual(1, peliculas.Count);
            Assert.AreEqual("No estrenada", peliculas[0].Titulo);
        }

        [TestMethod]
        public async Task FiltrarPorGenero()
        {
            var nombreBD = CrearDataPrueba();
            var contexto = ConstruitContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            var controller = new PelicuasController(contexto, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var generoId = contexto.Generos.Select(x => x.Id).First();
            var filtroDTO = new FiltroPeliculaDTO()
            {
                GeneroId = generoId,
            };
            var resultado = await controller.Filtrar(filtroDTO);
            var peliculas = resultado.Value;
            Assert.AreEqual(1, peliculas.Count);
            Assert.AreEqual("Pelicula con Genero", peliculas[0].Titulo);
        }

        [TestMethod]
        public async Task FiltrarOrdenaTituloAscendente()
        {
            var nombreBD = CrearDataPrueba();
            var contexto = ConstruitContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            var controller = new PelicuasController(contexto, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filtroDTO = new FiltroPeliculaDTO()
            {
                CampoOrdenar = "Titulo",
                OrdenAscendente = true
            };
            var respuesta = await controller.Filtrar(filtroDTO);
            var peliculas = respuesta.Value;
            var contexto2 = ConstruitContext(nombreBD);
            var peliculasDB = contexto2.Peliculas.OrderBy(p => p.Titulo).ToList();


            Assert.AreEqual(peliculasDB.Count, peliculas.Count);
            for (int i = 0; i < peliculasDB.Count; i++)
            {
                var peliculaDelControlador = peliculas[i];
                var peliculaDB = peliculasDB[i];
                Assert.AreEqual(peliculaDB.Id, peliculaDelControlador.Id);
            }
        }

        [TestMethod]
        public async Task FiltrarOrdenaTituloDesendente()
        {
            var nombreBD = CrearDataPrueba();
            var contexto = ConstruitContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            var controller = new PelicuasController(contexto, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var filtroDTO = new FiltroPeliculaDTO()
            {
                CampoOrdenar = "Titulo",
                OrdenAscendente = false
            };
            var respuesta = await controller.Filtrar(filtroDTO);
            var peliculas = respuesta.Value;
            var contexto2 = ConstruitContext(nombreBD);
            var peliculasDB = contexto2.Peliculas.OrderByDescending(p => p.Titulo).ToList();


            Assert.AreEqual(peliculasDB.Count, peliculas.Count);
            for (int i = 0; i < peliculasDB.Count; i++)
            {
                var peliculaDelControlador = peliculas[i];
                var peliculaDB = peliculasDB[i];
                Assert.AreEqual(peliculaDB.Id, peliculaDelControlador.Id);
            }
        }
        [TestMethod]
        public async Task FiltralPorCampoIncorrectoDevuelvePeliculas()
        {
            var nombreBD = CrearDataPrueba();
            var contexto = ConstruitContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            var mock = new Mock<ILogger<PelicuasController>>();

            var controller = new PelicuasController(contexto, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();


            var filtroDTO = new FiltroPeliculaDTO()
            {
                CampoOrdenar = "abc",
                OrdenAscendente = true
            };
            var respuesta = await controller.Filtrar(filtroDTO);
            var peliculas = respuesta.Value;
            var contexto2 = ConstruitContext(nombreBD);
            var peliculasDB = contexto2.Peliculas.ToList();

            Assert.AreEqual(peliculasDB.Count, peliculas.Count);
            Assert.AreEqual(1, mock.Invocations.Count);
        }
    }
}
