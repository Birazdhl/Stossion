using Stossion.BusinessLayers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MailKit.Security;
using MimeKit;
using Microsoft.IdentityModel.Protocols;
using Stossion.Helpers.Enum;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Stossion.BusinessLayers.Services
{
	public class EmailSenderService(IConfiguration configuration) : IEmailSenderService
	{
		public async Task SendEmailAsync(string toEmail, string subject, string message)
		{
			
			MailMessage Msg = new MailMessage();
			// Sender e-mail address.
			Msg.From = new MailAddress(configuration["EmailVerification:SenderEmail"] ?? string.Empty);
			// Recipient e-mail address.
			Msg.To.Add(toEmail);
			Msg.Subject = subject;
			Msg.IsBodyHtml = true;
			Msg.Body = message;

			SmtpClient smtp = new SmtpClient();
			smtp.EnableSsl = true;
			smtp.UseDefaultCredentials = false;
			NetworkCredential loginInfo = new NetworkCredential(configuration["EmailVerification:SenderEmail"] ?? string.Empty, configuration["EmailVerification:SenderKey"] ?? string.Empty); 
			smtp.Credentials = loginInfo;
			smtp.Host = "smtp.gmail.com";
			smtp.Port = 587;
			smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
			await smtp.SendMailAsync(Msg);
		}

	}
}
