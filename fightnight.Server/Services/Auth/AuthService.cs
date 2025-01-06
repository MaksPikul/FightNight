using Azure;
using Azure.Core;
using fightnight.Server.Abstracts;
using fightnight.Server.Builders;
using fightnight.Server.Dtos.Account;
using fightnight.Server.Factories;
using fightnight.Server.Interfaces;
using fightnight.Server.Interfaces.Auth;
using fightnight.Server.Interfaces.IServices;
using fightnight.Server.Models.Tables;
using fightnight.Server.Providers.EmailProviders;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace fightnight.Server.Services.Auth
{
    public class AuthService : IAuthService
    {
        protected readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IInviteService _inviteService;
        private readonly ITokenService _tokenService;
        private readonly IRegisterService _registerService;

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmailService emailService,
            IInviteService inviteService,
            ITokenService tokenService,
            IRegisterService registerService
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _inviteService = inviteService;
            _tokenService = tokenService;
            _registerService = registerService;
        }
        // Add User to Database not confirmed,
        // Add User to database confirmed
        // log user in - stays the same


        // Did someone just implement the strategy Pattern? ;P
        public async Task<AppUser> AddUnconfirmedUserAsync(AppUser appUser, string password = null)
        {
            return await _registerService.AddUnconfirmedUserAsync(appUser, password);
        }
        public async Task<AppUser> GetUnconfirmedUserAsync(string email)
        {
            return await _registerService.GetUnconfirmedUserAsync(email);
        }

        // These stay the same, regardless of whether im using redis or db to confirm users
        public async Task<IdentityResult> ConfirmUserAsync(AppUser appUser)
        {
            return await _registerService.ConfirmUserAsync(appUser);
        }

        public async Task<AppUser> LogUserInAsync(AppUser appUser, ClaimsPrincipal user, string password = null, bool rememberMe = false)
        {
            if (password == null)
            {
                await _signInManager.SignInAsync(appUser, isPersistent: true);
            }
            else
            {
                var identity = (ClaimsIdentity)user.Identity;
                identity.AddClaim(new Claim("Picture", appUser.Picture));

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
            // Seems unneccessary, but it takes the signInManager dependency out of account controller :D
            // Which means its necessary
            await _signInManager.SignOutAsync();
        }
    }

}



