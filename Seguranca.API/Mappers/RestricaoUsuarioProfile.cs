using AutoMapper;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Models;

namespace Seguranca.API.Mappers
{
    public class RestricaoUsuarioProfile : Profile
    {
        public RestricaoUsuarioProfile()
        {
            CreateMap<RestricaoUsuarioModel, RestricaoUsuario>().ReverseMap();
        }
    }
}
