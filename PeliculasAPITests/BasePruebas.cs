using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using PeliculasAPIs;
using PeliculasAPIs.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PeliculasAPITests
{
    public class BasePruebas
    {
        protected ApplicationDbContext ConstruitContext(string nombreDB)
        {
            var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(nombreDB).Options;

            var dbContext = new ApplicationDbContext(opciones);
            return dbContext;
        }

        protected IMapper ConfigurarAutoMapper()
        {
            var config = new MapperConfiguration(options =>
            {
                var geometryFactory = new NtsGeometryServices.Instance.GreateGeometryFactory(srid : 4326);  
                options.AddProfile(new AutoMapperProfiles(geometryFactory));
            });
            return config.CreateMapper();
        }
    }
}
