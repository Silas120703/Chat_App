﻿using MailKit.Net.Smtp;
using MimeKit;

namespace Chat_App_Shared.Helpers
{
	public class EmailHelper
	{
		private readonly IConfiguration _configuration;
		public EmailHelper(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public async Task SendEmailAsync(string email, string subject, string message)
		{
			var emailMessage = new MimeMessage();
			emailMessage.From.Add(new MailboxAddress("Ông chủ nhỏ của tôi " +
				"đã gửi cho bạn một bí mật nhỏ  ", _configuration["Smtp:Username"]));
			emailMessage.To.Add(new MailboxAddress("", email));
			emailMessage.Subject = subject;
			emailMessage.Body = new TextPart("plain") { Text = message };

			try
			{
				using (var client = new SmtpClient())
				{
					await client.ConnectAsync(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"]), false);
					await client.AuthenticateAsync(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);
					await client.SendAsync(emailMessage);
					await client.DisconnectAsync(true);
				}
				Console.WriteLine($"Email sent to {email}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to send email: {ex.Message}");
				throw;
			}
		}
	}
}
