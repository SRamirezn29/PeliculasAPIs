using Microsoft.EntityFrameworkCore;
using PeliculasAPIs.Servicios;

namespace PeliculasAPIs
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddTransient<AlmacenadorArchivoAzure, AlmacenadorArchivoAzure>();
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("DefaulConnection")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
