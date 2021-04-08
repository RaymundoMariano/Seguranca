using AutoMapper;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Models;

namespace Seguranca.API.Mappers
{
    public class UsuarioProfile : Profile
	{
		public UsuarioProfile()
		{
			CreateMap<UsuarioModel, Usuario>().ReverseMap();
		}
	}
}
