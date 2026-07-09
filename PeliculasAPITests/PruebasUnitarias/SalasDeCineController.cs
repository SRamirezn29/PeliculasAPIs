using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasAPIs.Controllers;
using PeliculasAPIs.DTOs;
using PeliculasAPIs.Entidades;
using PeliculasAPIs.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PeliculasAPITests.PruebasUnitarias
{
    [TestClass]
    public class SalasDeCineControllerPruebas : BasePruebas
    {
        [TestMethod]
        public async Task ObtenerSalasDeCineA5KilometrosOMenos()
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            using (var context = LocalDbDatabaseInitializer.GetDbContextLocalDb(false))
            {
                var salasDeCine = new List<SalaDeCine>()
                {
                    new SalaDeCine
                    {
                        Nombre = "Village East Cinema",
                        Ubicacion = geometryFactory.CreatePoint(new Coordinate(-74.0060, 40.7128))
                    }
                };

                context.AddRange(salasDeCine);
                await context.SaveChangesAsync();
            }

            var filtro = new SalaDeCineCercanoFiltroDTO()
            {
                DistanciaEnKms = 5,
                Latitud = 18.481139,
                Longitud = -69.938950
            };

            using (var context = LocalDbDatabaseInitializer.GetDbContextLocalDb(false))
            {
                var mapper = ConfigurarAutoMapper();
                var controller = new SalasDeCineController(context, mapper, geometryFactory);

                var respuesta = await controller.Cercanos(filtro);
                var valor = respuesta.Value;

                Assert.AreEqual(2, valor.Count);
            }
        }
    }
}