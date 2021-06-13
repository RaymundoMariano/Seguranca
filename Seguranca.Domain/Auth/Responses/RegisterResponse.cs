using System.Collections.Generic;

namespace Seguranca.Domain.Auth.Responses
{
    public class RegisterResponse
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public bool Authenticated { get; set; }
        public int ObjectResult { get; set; }
        public List<string> Errors { get; set; }
    }
}
