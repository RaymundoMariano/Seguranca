namespace Seguranca.Domain.Entities
{
    public partial class PerfilUsuario : Entity
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ModuloId { get; set; }
        public int? PerfilId { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual Modulo Modulo { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
