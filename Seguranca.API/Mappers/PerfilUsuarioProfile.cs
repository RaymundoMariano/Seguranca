using AutoMapper;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Models;

namespace Seguranca.API.Mappers
{
    public class PerfilUsuarioProfile : Profile
    {
        public PerfilUsuarioProfile()
        {
            CreateMap<PerfilUsuarioModel, PerfilUsuario>().ReverseMap();
        }
    }
}
