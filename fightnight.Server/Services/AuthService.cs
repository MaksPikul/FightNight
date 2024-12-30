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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace fightnight.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IInviteService _inviteService;
        private readonly ITokenService _tokenService;
        
        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmailService emailService,
            IInviteService inviteService,
            ITokenService tokenService  
        ) {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _inviteService = inviteService;
            _tokenService = tokenService;
        }

        public async Task<AppUser> RegisterUserAsync(AppUser appUser, string password = null)
        {
            // you can pass in nulls for password,
            // I just wanted to show here that u can create with nulls
            IdentityResult createResult = (password == null) ? 
                await _userManager.CreateAsync(appUser) 
                : 
                await _userManager.CreateAsync(appUser, password);
            
            if (!createResult.Succeeded)
            {
                throw new Exception("Internal Error Creating User");
            }

            IdentityResult roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            if (!roleResult.Succeeded)
            {
                throw new Exception("Internal Error Adding Role to User");
            }

            return appUser;
        }

        public async Task<AppUser> LogUserInAsync(AppUser appUser, string password = null, bool rememberMe = false)
        {
            if (password == null)
            {
                await _signInManager.SignInAsync(appUser, isPersistent: true);
            }
            else
            {
                var SignInResult = await _signInManager.PasswordSignInAsync(appUser.UserName, password, rememberMe, lockoutOnFailure: true);

                if (SignInResult.IsLockedOut)
                {
                    //Send Email to this user saying someone is trying to access your account
                    throw new UnauthorizedAccessException("Account is locked");
                }
                else if (!SignInResult.Succeeded)
                {
                    throw new UnauthorizedAccessException("Invalid Credentials");
                }
            }
            return appUser;
        }

        public async void LogUserOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }

}



