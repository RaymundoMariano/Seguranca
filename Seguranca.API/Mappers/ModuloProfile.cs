using AutoMapper;
using Seguranca.Domain.Models;
using Seguranca.Domain.Entities;

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
