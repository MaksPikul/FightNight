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
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IInviteService _inviteService;
        private readonly ITokenService _tokenService;
        
        public AuthService(
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

        public async Task<AppUser> RegisterUserAsync(AppUser appUser, RegisterDto registerDto)
        {
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

            return appUser;
        }

        public Task<AppUser> LogUserInAsync()
        {
            throw new NotImplementedException();
        }
    }

}



