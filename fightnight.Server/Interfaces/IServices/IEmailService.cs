using fightnight.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace fightnight.Server.Interfaces.IServices
{
    public interface IEmailService
    {
        Task Send(ConfirmEmailTemplate emailMetaData);
    }
}
