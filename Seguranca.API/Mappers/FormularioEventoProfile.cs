using AutoMapper;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Models;

namespace Seguranca.API.Mappers
{
    public class FormularioEventoProfile : Profile
    {
        public FormularioEventoProfile()
        {
            CreateMap<FormularioEventoModel, FormularioEvento>().ReverseMap();
        }
    }
}
