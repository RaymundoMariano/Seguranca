using AutoMapper;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Models;

namespace Seguranca.API.Mappers
{
    public class ModuloFormularioProfile : Profile
    {
        public ModuloFormularioProfile()
        {
            CreateMap<ModuloFormularioModel, ModuloFormulario>().ReverseMap();
        }
    }
}
