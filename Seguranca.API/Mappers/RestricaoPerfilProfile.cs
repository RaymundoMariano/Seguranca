using AutoMapper;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Models;

namespace Seguranca.API.Mappers
{
    public class RestricaoPerfilProfile : Profile
    {
        public RestricaoPerfilProfile()
        {
            CreateMap<RestricaoPerfilModel, RestricaoPerfil>().ReverseMap();
        }
    }
}
