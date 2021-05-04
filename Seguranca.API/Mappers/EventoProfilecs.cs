using AutoMapper;
using Seguranca.Domain.Models;
using Seguranca.Domain.Entities;

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
