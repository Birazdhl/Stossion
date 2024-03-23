using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.BusinessLayers.Interfaces
{
	public interface IEmailSenderService
	{
		Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
