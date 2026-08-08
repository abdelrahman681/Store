
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Store.CoreLayer.IServices;
using Store.CoreLayer.Entirty.Mail;

namespace Store.Service
{
    public class EmailSetting :IEmailSetting
    {
        private readonly MailSetting _option;

        public EmailSetting(IOptions<MailSetting> option)
        {
            _option = option.Value;
        }

        public async Task SendAsyncByUsingMailKite(Email email)
        {
            var mail = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_option.Email),
                Subject = email.Subject,
            };
            mail.To.Add(MailboxAddress.Parse(email.To));
            mail.From.Add(new MailboxAddress(_option.DisplayName, _option.Email));
            var bulider = new BodyBuilder();
            bulider.TextBody = email.Body;
            mail.Body = bulider.ToMessageBody();
            using var clint = new SmtpClient();
            await clint.ConnectAsync(_option.Host, _option.Port, SecureSocketOptions.StartTls);
            await clint.AuthenticateAsync(_option.Email, _option.Password);
            await clint.SendAsync(mail);
            await clint.DisconnectAsync(true);
        }
    }
}
