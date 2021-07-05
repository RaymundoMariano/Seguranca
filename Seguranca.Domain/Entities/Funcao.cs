namespace Seguranca.Domain.Entities
{
    public partial class Funcao : Entity
    {
        public int FuncaoId { get; set; }
        public string Nome { get; set; }
        public bool CreatedSystem { get; set; }

        public virtual Perfil Perfil { get; set; }
    }
}
