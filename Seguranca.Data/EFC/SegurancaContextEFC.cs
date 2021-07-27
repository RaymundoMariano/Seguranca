using Microsoft.EntityFrameworkCore;
using Seguranca.Data.EFC.Tables;
using Seguranca.Domain.Contracts.Repositories.Seedwork;
using Seguranca.Domain.Entities;
using System.Threading.Tasks;

namespace Seguranca.Data.EFC
{
    public partial class SegurancaContextEFC : DbContext, IUnitOfWork
    {
        public SegurancaContextEFC()
        {
        }

        public SegurancaContextEFC(DbContextOptions<SegurancaContextEFC> options)
            : base(options)
        {
            new DbInitializer(this);
        }

        public virtual DbSet<Evento> Eventos { get; set; }
        public virtual DbSet<Formulario> Formularios { get; set; }
        public virtual DbSet<FormularioEvento> FormulariosEvento { get; set; }
        public virtual DbSet<Funcao> Funcoes { get; set; }
        public virtual DbSet<Modulo> Modulos { get; set; }
        public virtual DbSet<ModuloFormulario> ModulosFormulario { get; set; }
        public virtual DbSet<Perfil> Perfis { get; set; }
        public virtual DbSet<PerfilUsuario> PerfisUsuario { get; set; }
        public virtual DbSet<RestricaoPerfil> RestricoesPerfil { get; set; }
        public virtual DbSet<RestricaoUsuario> RestricoesUsuario { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer("Server=DESKTOP-S3R5UB7\\SQLEXPRESS;Database=DesBd_Seguranca;Trusted_Connection=True;");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.ApplyConfiguration(new EventoTable());
            modelBuilder.ApplyConfiguration(new FormularioTable());
            modelBuilder.ApplyConfiguration(new FormularioEventoTable());
            modelBuilder.ApplyConfiguration(new FuncaoTable());
            modelBuilder.ApplyConfiguration(new ModuloTable());
            modelBuilder.ApplyConfiguration(new ModuloFormularioTable());
            modelBuilder.ApplyConfiguration(new PerfilTable());
            modelBuilder.ApplyConfiguration(new PerfilUsuarioTable());
            modelBuilder.ApplyConfiguration(new RestricaoPerfilTable());
            modelBuilder.ApplyConfiguration(new RestricaoUsuarioTable());
            modelBuilder.ApplyConfiguration(new UsuarioTable());           

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
