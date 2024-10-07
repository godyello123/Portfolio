using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace SCommon
{
	public class SSendMail
	{
		public string m_Sender;
		public string m_Password;
		public string m_SMTP;
		public int m_SMTPPort;
		public bool m_EnableSSL;

		public SSendMail(string sender, string password, string smtp, int smtpPort, bool enableSSL = true)
		{
			m_Sender = sender;
			m_Password = password;
			m_SMTP = smtp;
			m_SMTPPort = smtpPort;
			m_EnableSSL = enableSSL;
		}

		public bool SendMail(string receiver, string subject, string body)
		{
			MailMessage mail = new MailMessage();
			mail.From = new MailAddress(m_Sender);
			mail.To.Add(receiver);
			mail.Subject = subject;
			mail.Body = body;
			SmtpClient smtp = new SmtpClient(m_SMTP, m_SMTPPort);
			smtp.EnableSsl = m_EnableSSL;
			smtp.Credentials = new NetworkCredential(m_Sender, m_Password);
			try
			{
				smtp.Send(mail);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public bool SendMail(List<string> receiverList, string subject, string body)
		{
			MailMessage mail = new MailMessage();
			mail.From = new MailAddress(m_Sender);
			foreach(var data in receiverList) mail.To.Add(data);
			mail.Subject = subject;
			mail.Body = body;
			SmtpClient smtp = new SmtpClient(m_SMTP, m_SMTPPort);
			smtp.EnableSsl = m_EnableSSL;
			smtp.Credentials = new NetworkCredential(m_Sender, m_Password);
			try
			{
				smtp.Send(mail);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
