using Seguranca.Domain.Auth.Requests;
using Seguranca.Domain.Contracts.Clients.Auth;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Seguranca.Client.Auth
{
    public class RegisterClient : Api, IRegisterClient
    {
        public RegisterClient() : base("https://localhost:44305/api/usuarios/register") { }

        public async Task<HttpResponseMessage> RegisterAsync(RegisterRequest register)
        {
            base.NovaRota("", null);
            return await base.Client.PostAsJsonAsync("", register);
        }
    }
}
 