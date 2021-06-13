using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Seguranca.Client.Auth;
using Seguranca.Data.EFC;
using Seguranca.Data.EFC.Repositories;
using Seguranca.Domain.Contracts.Clients.Auth;
using Seguranca.Domain.Contracts.Repositories;
using Seguranca.Domain.Contracts.Services;
using Seguranca.Service;
using System.Text;

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

            services.AddTransient<IPerfilUsuarioRepository, PerfilUsuarioRepositoryEFC>();
            services.AddTransient<IRestricaoUsuarioRepository, RestricaoUsuarioRepositoryEFC>();
            services.AddTransient<IRestricaoPerfilRepository, RestricaoPerfilRepositoryEFC>();
            services.AddTransient<IModuloFormularioRepository, ModuloFormularioRepositoryEFC>();
            services.AddTransient<IFormularioEventoRepository, FormularioEventoRepositoryEFC>();

            services.AddTransient<IModuloRepository, ModuloRepositoryEFC>();
            services.AddTransient<IModuloService, ModuloService>();
            services.AddTransient<IPerfilRepository, PerfilRepositoryEFC>();
            services.AddTransient<IPerfilService, PerfilService>();
            services.AddTransient<IUsuarioRepository, UsuarioRepositoryEFC>();
            services.AddTransient<IUsuarioService, UsuarioService>();

            services.AddTransient<IRegisterClient, RegisterClient>();
            services.AddTransient<ILoginClient, LoginClient>();

            //Controllers protegidos contra acesso anônimo exceto as actions que tenham o atributo
            services.AddControllersWithViews(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                       .RequireAuthenticatedUser()
                       .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            // injeção dependência mappers
            services.AddAutoMapper(typeof(Startup).Assembly);

            //injeção de dependência NewsoftJson - Microsoft.AspNetCore.Mvc.NewtonsoftJson
            services.AddControllers()
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                    .AddNewtonsoftJson(options => options.SerializerSettings.NullValueHandling =
                        Newtonsoft.Json.NullValueHandling.Ignore);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    // Validar a terceira parte do token jwt usando o segredo que adicionamos
                    // no appsettings e verifica se o token jwt foi gerado
                    // https://www.browserling.com/tools/random-string <- Gera o segredo aleatoriamente
                    ValidateIssuerSigningKey = true,

                    // Adiciona chave secreta à nossa criptografia Jwt
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Seguranca.API", Version = "v1" });
            });
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
