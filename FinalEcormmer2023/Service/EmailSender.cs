using System.Net.Mail;
using System.Net;
using FinalEcormer2023.Interfaces;

namespace FinalEcormer2023.Service {
	public class EmailSender : IEmailSender {
		public Task SendEmailAsync(string email, string subject, string htmlMessage) {
			SmtpClient client = new SmtpClient {
				Port = 587,
				Host = "smtp.gmail.com", //or another email sender provider
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential("nguyenhuyhoa2003@gmail.com", "bowp dtbs ekzc jumz")
			};
			MailMessage message = new MailMessage("nguyenhuyhoa2003@gmail.com", email, subject, htmlMessage);
			message.IsBodyHtml = true;	

			return client.SendMailAsync(message);
		}
	}
}
