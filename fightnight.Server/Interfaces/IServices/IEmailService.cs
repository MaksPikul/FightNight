using fightnight.Server.Abstracts;
using fightnight.Server.Services;
using FluentEmail.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace fightnight.Server.Interfaces.IServices
{
    public interface IEmailService
    {
        Task<SendResponse> Send(Email emailMetaData);
    }
}
