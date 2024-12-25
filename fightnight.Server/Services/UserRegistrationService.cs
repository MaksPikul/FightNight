using Azure;
using fightnight.Server.Abstracts;
using fightnight.Server.Builders;
using fightnight.Server.Dtos.Account;
using fightnight.Server.Factories;
using fightnight.Server.Interfaces;
using fightnight.Server.Interfaces.IServices;
using fightnight.Server.Models.Tables;
using fightnight.Server.Providers.EmailProviders;
using FluentEmail.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace fightnight.Server.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IInviteService _inviteService;
        private readonly ITokenService _tokenService;
        
        public UserRegistrationService(
            UserManager<AppUser> userManager,
            IEmailService emailService,
            IInviteService inviteService,
            ITokenService tokenService  
        ) {
            _userManager = userManager;
            _emailService = emailService;
            _inviteService = inviteService;
            _tokenService = tokenService;
        }
        public async Task<string> RegisterUserAsync(RegisterDto registerDto, HttpResponse response)
        {

            AppUser appUser = AppUserFactory.Create(registerDto);

            var createResult = await _userManager.CreateAsync(appUser, registerDto.Password);
            if (!createResult.Succeeded)
            {
                throw new Exception("Internal Error Creating User");
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            if (!roleResult.Succeeded)
            {
                throw new Exception("Internal Error Adding Role to User");
            }

            if (registerDto.inviteId != null)
            {
                _inviteService.HandleInvitation(appUser, registerDto.inviteId, response);
                await _userManager.UpdateAsync(appUser);
            }

            if (!appUser.EmailConfirmed)
            {
                // Create Email
                Email email = new RegisterConfirmEmail(registerDto.Email, _tokenService);

                // Send Email
                SendResponse emailResult = await _emailService.Send(email);
                if (!emailResult.Successful) 
                { 
                    throw new Exception("Email Failed to Send, Message ID: " + emailResult.MessageId + " Errors: " + emailResult.ErrorMessages); 
                }
            }

            return "User Registered Successfully";
        }
    }

}



