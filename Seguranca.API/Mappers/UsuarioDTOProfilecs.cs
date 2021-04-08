using AutoMapper;
using Seguranca.Domain.DTO;
using Seguranca.Domain.Entities;

namespace Seguranca.API.Mappers
{
    public class UsuarioDTOProfile : Profile
	{
		public UsuarioDTOProfile()
		{
			CreateMap<UsuarioDTO, Usuario>().ReverseMap();
		}
	}
}
