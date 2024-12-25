using fightnight.Server.Interfaces.IServices;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace fightnight.Server.Services
{
    public class EmailService : IEmailService
    {
        private readonly IFluentEmail _fluentEmail;
        public EmailService(IFluentEmail fluentEmail) 
        {
            _fluentEmail = fluentEmail 
                ?? throw new ArgumentNullException(nameof(fluentEmail));
        }
        public async Task<SendResponse> Send(Abstracts.Email emailMetaData)
        {
            return await _fluentEmail
                .To(emailMetaData.Recipient)
                .Subject(emailMetaData.Subject)
                .Body(emailMetaData.Body, isHtml:true)
                .SendAsync();
        }
    }
    public class EmailSettings
    {
        public string DefaultSenderEmail { get; set; }
        public string DefaultSenderName { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
    }
}
