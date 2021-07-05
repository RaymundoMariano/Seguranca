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

        public virtual DbSet<Evento> Evento { get; set; }
        public virtual DbSet<Formulario> Formulario { get; set; }
        public virtual DbSet<FormularioEvento> FormularioEvento { get; set; }
        public virtual DbSet<Funcao> Funcao { get; set; }
        public virtual DbSet<Modulo> Modulo { get; set; }
        public virtual DbSet<ModuloFormulario> ModuloFormulario { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }
        public virtual DbSet<PerfilUsuario> PerfilUsuario { get; set; }
        public virtual DbSet<RestricaoPerfil> RestricaoPerfil { get; set; }
        public virtual DbSet<RestricaoUsuario> RestricaoUsuario { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        
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
