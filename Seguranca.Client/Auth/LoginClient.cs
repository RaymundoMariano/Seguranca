using Seguranca.Domain.Auth.Requests;
using Seguranca.Domain.Contracts.Clients.Auth;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Seguranca.Client.Auth
{
    public class LoginClient : Api, ILoginClient
    {
        public LoginClient() : base("https://localhost:44305/api/usuarios/login") { }

        public async Task<HttpResponseMessage> LoginAsync(LoginRequest login)
        {
            base.NovaRota("", null);
            return await base.Client.PostAsJsonAsync("", login);         
        }
    }
} 
