using AutoMapper;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Models;

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
