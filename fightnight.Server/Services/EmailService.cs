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
        public async Task<SendResponse> SendEmail(Abstracts.Email email)
        {
            SendResponse response = await _fluentEmail
                .To(email.Recipient)
                .Subject(email.Subject)
                .Body(email.Body, isHtml:true)
                .SendAsync();

            if (!response.Successful)
            {
                throw new Exception("Email Failed to Send, Message ID: " + response.MessageId + " Errors: " + response.ErrorMessages);
            }

            return response;
        }
    }
    
}
