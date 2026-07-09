using AutoMapper;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using PeliculasAPIs;
using PeliculasAPIs.DTOs;
using PeliculasAPIs.Helpers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace PeliculasAPITests
{
    public class BasePruebas
    {
        protected string usuarioPorDefectoId = "40c5ae74-484f-4990-998d-b0e0a77eeeed";
        protected string usuarioPorDefectoEmail = "ejemplo@hotmail.com";

        protected ApplicationDbContext ConstruirtContext(string nombreDB)
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

        protected ControllerContext ConstruirControllerContext()
        {
            var usuario = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, usuarioPorDefectoEmail),
                new Claim(ClaimTypes.Email, usuarioPorDefectoEmail),
                new Claim(ClaimTypes.NameIdentifier, usuarioPorDefectoId)
            }));

            return new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = usuario },
            };
        }

        protected WebApplicationFactory<Startup> ConstruirWebApplicationFactory( string nombreDB,
            bool ignorarSeguridad = true)
        {
            var factory = new WebApplicationFactory<Startup>();
            factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var descriptorDBContext = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptorDBContext != null)
                    {
                        services.Remove(descriptorDBContext);
                    }
                    services.AddDbContext<ApplicationDbContext>(options => 
                    options.UseInMemoryDatabase(nombreDB));

                    if (ignorarSeguridad)
                    {
                        services.AddSingleton<IAuthorizationHandler, AllowAnonymousHandler>();

                        services.AddControllers(options =>
                        {
                            options.Filters.Add(new UsuarioFalsoFiltro());
                        });
                    }
                });
            });

            return factory;
        }
    }
}
