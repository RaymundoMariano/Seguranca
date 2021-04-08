using AutoMapper;
using Seguranca.Domain.Entities;
using Seguranca.Domain.Models;

namespace Seguranca.API.Mappers
{
    public class EventoProfile : Profile
	{
		public EventoProfile()
		{
			CreateMap<EventoModel, Evento>().ReverseMap();
		}
	}
}
