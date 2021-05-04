using AutoMapper;
using Seguranca.Domain.Models;
using Seguranca.Domain.Entities;

namespace Seguranca.API.Mappers
{
    public class FormularioProfile : Profile
	{
		public FormularioProfile()
		{
			CreateMap<FormularioModel, Formulario>().ReverseMap();
		}
	}
}
