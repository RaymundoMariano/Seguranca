using AutoMapper;
using Seguranca.Domain.Models;
using Seguranca.Domain.Entities;

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
