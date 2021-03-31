using System;

namespace Seguranca.Domain.Contracts.Services
{
    public class ServiceException : Exception
	{
		public ServiceException(String mensagem, Exception inner) : base(mensagem, inner)
		{ }

		public ServiceException(String mensagem) : base(mensagem)
		{ }
	}
}
