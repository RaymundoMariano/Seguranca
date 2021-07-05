using AutoMapper;
using Seguranca.Domain.Models;
using Seguranca.Domain.Entities;

namespace Seguranca.API.Mappers
{
    public class FuncaoProfile : Profile
	{
		public FuncaoProfile()
		{
			CreateMap<FuncaoModel, Funcao>().ReverseMap();
		}
	}
}
