using Seguranca.Domain.Auth.Requests;
using System.Net.Http;
using System.Threading.Tasks;

namespace Seguranca.Domain.Contracts.Clients.Auth
{
    public interface ILoginClient
    {
        Task<HttpResponseMessage> LoginAsync(LoginRequest login);
    }
}
