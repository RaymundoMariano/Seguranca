using AutoMapper;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Models;

namespace Seguranca.API.Mappers
{
    public class PerfilProfile : Profile
	{
		public PerfilProfile()
		{
			CreateMap<PerfilModel, Perfil>().ReverseMap();
		}
	}
}
