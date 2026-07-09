using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PeliculasAPIs.Controllers;
using PeliculasAPIs.DTOs;
using PeliculasAPIs.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeliculasAPITests.PruebasUnitarias
{
    public class ReviewsControllerTest
    {
        [TestMethod]
        public async Task UsuarioNoPuedeCrearDosReviewsParaLaMismaPelicula()
        {
            var nombreDB = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreDB);
            CrearPeliculas(nombreDB);

            var peliculaId = contexto.Peliculas.Select(x => x.Id).First();
            var review1 = new Review()
            {
                peliculaId = peliculaId,
                UsuarioId = usuarioPorDefectoId,
                Puntuacion = 5
            };
            contexto.Add(review1);
            await contexto.SaveChangesAsync();

            var contexto2 = ConstruirContext(nombreBD);
            var mapper = ConfigurarAutoMapper();

            var controller = new ReviewController(contexto2, mapper);
            controller.ControllerContext = ConstruirControllerContext();

            var reviewCreacionDTO = new ReviewCreasionDTO { Puntuacion = 5 };
            var respuesta = await controller.Post(peliculaId, reviewCreacionDTO);

            var valor = respuesta as IStatusCodeActionResult;
            Assert.AreEqual(400, valor.StatusCode.Value);
        }

        [TestMethod]
        public async Task CrearReview()
        {
            var nombreBD = Guid.NewGuid().ToString();
            var contexto = ConstruirContext(nombreBD);
            CrearPeliculas(nombreBD);

            var peliculaId = contexto.Peliculas.Select(x => x.Id).First();
            var contexto2 = ConstruirContext(nombreBD);

            var mapper = ConfigurarAutoMapper();
            var controller = new ReviewController(contexto2, mapper);
            controller.ControllerContext = ConstruirControllerContext();

            var reviewCreacionDTO = new ReviewCreasionDTO() { Puntuacion = 5 };
            var respuesta = await controller.Post(peliculaId, reviewCreacionDTO);

            var valor = respuesta as NoContentResult;
            Assert.IsNotNull(valor);

            var contexto3 = ConstruirContext(nombreBD);
            var reviewDB = contexto3.Reviews.First();
            Assert.AreEqual(usuarioPorDefectoId, reviewDB.UsuarioId);
        }




        private void CrearPeliculas(string nombreDB)
        {
            var contexto = ConstruirContext(nombreDB);
            contexto.Peliculas.Add(new Pelicula() { Titulo = "pelicula 1" });
            contexto.SaveChanges();
        }
    }
}
