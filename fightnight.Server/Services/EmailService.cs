using fightnight.Server.Interfaces.IServices;
using FluentEmail.Core;
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
        public async Task Send(ConfirmEmailTemplate emailMetaData)
        {
            
            await _fluentEmail
                .To(emailMetaData.SendingTo)
                .Subject(emailMetaData.EmailSubject)
                .Body(emailMetaData.EmailBody, isHtml:true)
                .SendAsync();
            

            //Figure out how to return success and error messages
            //return Ok(res);
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

    public class ConfirmEmailTemplate
    {
        public string SendingTo { get; set; }
        public string? EmailSubject { get; set; } = "Confirm your Fight Night Account ";
        public string? EmailBody { get; set; } 
        public string? EmailAttachmentPath { get; set; }

        
    }
}
