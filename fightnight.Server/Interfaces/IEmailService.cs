using fightnight.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace fightnight.Server.Interfaces
{
    public interface IEmailService
    {
        Task Send(ConfirmEmailTemplate emailMetaData);
    }
}
