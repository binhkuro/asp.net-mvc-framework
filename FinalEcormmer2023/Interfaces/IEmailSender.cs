﻿namespace FinalEcormer2023.Interfaces {
	public interface IEmailSender {
		public Task SendEmailAsync(string email, string subject, string htmlMessage);
	}

}