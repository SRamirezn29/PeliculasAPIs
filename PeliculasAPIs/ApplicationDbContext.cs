using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using PeliculasAPIs.Entidades;
using PeliculasAPIs.Migrations;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace PeliculasAPIs
{
    public class ApplicationDbContext: IdentityDbContext 
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeliculasActores>().HasKey(x => new { x.ActorId, x.PeliculaId });
            modelBuilder.Entity<PeliculasGenero>().HasKey(x => new { x.GeneroId, x.PeliculaId });
            modelBuilder.Entity<PeliculasSalasDeCine>().HasKey(x => new { x.PeliculaId, x.SalaDeCineId });

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public void SeedData(ModelBuilder modelBuilder)
        {
            var rolAdmindId = "fecfa021-77d5-4ee9-999f-cc2c74498dbf";
            var usuarioAdminId = "1cdf717d-838a-46bd-9a13-a8274fe47a08";

            var rolAdmin = new IdentityRole()
            {
                Id = rolAdmindId,
                Name = "admin",
                NormalizedName = "admin"
            };

            var passwordHasher = new PasswordHasher<IdentityUser>();
            var username = "felipe@hotmail.com";

            var usuarioAdmin = new IdentityUser()
            {
                Id = usuarioAdminId,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "admin@example.com",
                NormalizedEmail = "admin@example.com",
                PasswordHash = passwordHasher.HashPassword(null, "Password123!")
            };

            //modelBuilder.Entity<IdentityUser>()
            //    .HasData(usuarioAdmin);

            //modelBuilder.Entity<IdentityRole>()
            //    .HasData(rolAdmin);

            //modelBuilder.Entity<IdentityUserClaim<string>>()
            //    .HasData(new IdentityUserClaim<string>()
            //    {
            //            Id = 1,
            //            ClaimType = ClaimTypes.Role,
            //            UserId = usuarioAdminId,
            //            ClaimValue = "admin"
            //    });

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            modelBuilder.Entity<SalaDeCine>()
           .HasData(new List<SalaDeCine>
            {
                new SalaDeCine{Id = 4, Nombre = "Sambil", Ubicacion = geometryFactory.CreatePoint(new Coordinate(-74.0060, 40.7128))},
                new SalaDeCine{Id = 5, Nombre = "Megacentro", Ubicacion = geometryFactory.CreatePoint(new Coordinate(-74.0060, 40.7128))},
                new SalaDeCine{Id = 6, Nombre = "Village East Cinema", Ubicacion = geometryFactory.CreatePoint(new Coordinate(-74.0060, 40.7128))}
            });


            var aventura = new Genero() { Id = 1, Nombre = "Aventura" };
            var comedia = new Genero() { Id = 2, Nombre = "Comedia" };
            var drama = new Genero() { Id = 3, Nombre = "Drama" };
            var terror = new Genero() { Id = 4, Nombre = "Terror" };
            var romance = new Genero() { Id = 5, Nombre = "Romance" };

            modelBuilder.Entity<Genero>()
                .HasData(new List<Genero>
                {
                    aventura, comedia, drama, terror, romance
                });

            var jimCarrey = new Actor() { Id = 5, Nombre = "Jim Carrey", FechaNacimiento = new DateTime(1962, 01, 17) };
            var robertDowneyJr = new Actor() { Id = 6, Nombre = "Robert Downey Jr.", FechaNacimiento = new DateTime(1965, 04, 04) };
            var scarlettJohansson = new Actor() { Id = 7, Nombre = "Scarlett Johansson", FechaNacimiento = new DateTime(1984, 11, 22) };

            modelBuilder.Entity<Actor>()
                .HasData(new List<Actor>
                {
                    jimCarrey, robertDowneyJr, scarlettJohansson
                });

            var endgame = new Pelicula()
            {
                Id = 2,
                Titulo = "Avengers: Endgame",
                FechaEstreno = new DateTime(2019, 04, 26),
            };

            var iw = new Pelicula()
            {
                Id = 3,
                Titulo = "Avangers: Infinity wars",
                EnCines = false,
                FechaEstreno = new DateTime(2019, 04, 26)
            };

            var sonic = new Pelicula()
            {
                Id = 4,
                Titulo = "Sonic the Hedgehog",
                EnCines = false,
                FechaEstreno = new DateTime(2019, 04, 26)
            };

            var emma = new Pelicula()
            {
                Id = 5,
                Titulo = "Emma",
                EnCines = false,
                FechaEstreno = new DateTime(2020, 02, 21)
            };

            var wonderwaman = new Pelicula()
            {
                Id = 6,
                Titulo = "Wander waman 1984",
                EnCines = false,
                FechaEstreno = new DateTime(2020, 08, 14)
            };

            modelBuilder.Entity<Pelicula>()
                .HasData(new List<Pelicula>
                {
                    endgame, iw, sonic, emma, wonderwaman
                });

            modelBuilder.Entity<PeliculasGenero>().HasData(
             new List<PeliculasGenero>()
             {
                 new PeliculasGenero() {PeliculaId = endgame. Id, GeneroId = terror.Id},
                 new PeliculasGenero() {PeliculaId = endgame. Id, GeneroId = aventura.Id},
                 new PeliculasGenero() {PeliculaId = iw. Id, GeneroId = terror.Id},
                 new PeliculasGenero() {PeliculaId = iw. Id, GeneroId = aventura. Id},
                 new PeliculasGenero() {PeliculaId = sonic. Id, GeneroId = romance .Id},
                 new PeliculasGenero() {PeliculaId = emma. Id, GeneroId = romance.Id},
                 new PeliculasGenero() {PeliculaId = wonderwaman. Id, GeneroId = terror. Id},
                 new PeliculasGenero() {PeliculaId = wonderwaman. Id, GeneroId = aventura.Id},
             });


            modelBuilder.Entity<PeliculasActores>().HasData(
                new List<PeliculasActores>()
                {
                    new PeliculasActores() { PeliculaId = endgame.Id, ActorId = robertDowneyJr.Id, Personaje = "Tony Stark" },
                    new PeliculasActores() {PeliculaId = endgame.Id, ActorId = scarlettJohansson.Id, Personaje = "Scarlett Johansson "},
                    new PeliculasActores() { PeliculaId = iw.Id, ActorId = robertDowneyJr.Id, Personaje = "Tony Stark" }
                });
        }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<PeliculasActores> PeliculasActores { get; set; }
        public DbSet<PeliculasGenero> PeliculasGeneros { get; set; }
        public DbSet<SalaDeCine> SalasDeCine { get; set; }
        public DbSet<Review > Reviews { get; set; }
    }
}
