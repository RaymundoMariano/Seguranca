using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Seguranca.Data.EFC;
using Seguranca.Data.EFC.Repositories;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Service;

namespace Seguranca.API
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
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Seguranca.API", Version = "v1" });
            });

            // injeção dependência DBContext
            services.AddDbContext<SegurancaContextEFC>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SegurancaConnection")));

            // injeção dependência Services
            services.AddScoped<SegurancaContextEFC>();
            services.AddTransient<IEventoRepository, EventoRepositoryEFC>();
            services.AddTransient<IEventoService, EventoService>();
            services.AddTransient<IFormularioRepository, FormularioRepositoryEFC>();
            services.AddTransient<IFormularioService, FormularioService>();
            services.AddTransient<IModuloRepository, ModuloRepositoryEFC>();
            services.AddTransient<IModuloService, ModuloService>();
            services.AddTransient<IPerfilRepository, PerfilRepositoryEFC>();
            services.AddTransient<IPerfilService, PerfilService>();
            services.AddTransient<IUsuarioRepository, UsuarioRepositoryEFC>();
            services.AddTransient<IUsuarioService, UsuarioService>();

            // injeção dependência mappers
            services.AddAutoMapper(typeof(Startup).Assembly);

            //injeção de dependência NewsoftJson - Microsoft.AspNetCore.Mvc.NewtonsoftJson
            services.AddControllers()
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                    .AddNewtonsoftJson(options => options.SerializerSettings.NullValueHandling =
                        Newtonsoft.Json.NullValueHandling.Ignore);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Seguranca.API v1"));
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
