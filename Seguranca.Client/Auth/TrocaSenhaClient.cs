using Seguranca.Domain.Auth.Requests;
using Seguranca.Domain.Contracts.Clients.Auth;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Seguranca.Client.Auth
{
    public class TrocaSenhaClient : Api, ITrocaSenhaClient
    {
        public TrocaSenhaClient() : base("https://localhost:44305/api/usuarios/trocasenha") { }

        public async Task<HttpResponseMessage> TrocaSenhaAsync(TrocaSenhaRequest trocaSenha)
        {
            base.NovaRota("", null);
            return await base.Client.PostAsJsonAsync("", trocaSenha);         
        }
    }
} 
