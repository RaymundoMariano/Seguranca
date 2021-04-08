using AutoMapper;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Models;

namespace Seguranca.API.Mappers
{
    public class ModuloProfile : Profile
	{
		public ModuloProfile()
		{
			CreateMap<ModuloModel, Modulo>().ReverseMap();
		}
	}
}
