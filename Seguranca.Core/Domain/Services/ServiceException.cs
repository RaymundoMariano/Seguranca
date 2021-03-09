using System;

namespace Seguranca.Core.Domain.Services
{
    public class ServiceException : Exception
    {
		public ServiceException(String mensagem, Exception inner)
			: base(mensagem, inner)
		{

		}

		public ServiceException(String mensagem)
			: base(mensagem)
		{

		}
	}
}
