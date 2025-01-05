using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Distributed;
using fightnight.Server.Interfaces;
using fightnight.Server.Models.Tables;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using fightnight.Server.Interfaces.IServices;

namespace fightnight.Server.Services
{
    public class CacheRegisterService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IDistributedCache cache,
        IEmailService emailService,
        IInviteService inviteService,
        ITokenService tokenService,
        IRegisterService registerService
    )
        :
    AuthService(userManager, signInManager, emailService, inviteService, tokenService, registerService), IRegisterService
    {
        private readonly IDistributedCache _cache = cache;
        private readonly PasswordHasher<AppUser> _passwordHasher = new PasswordHasher<AppUser>();

        public async Task<AppUser> AddUnconfirmedUserAsync(AppUser appUser, string password = null)
        {
            if (password != null) {
                appUser.PasswordHash = _passwordHasher.HashPassword(appUser, password);
            }
            
            string userData = JsonSerializer.Serialize(appUser);

            await _cache.SetStringAsync(appUser.Email, userData);

            return appUser;
        }

        public async Task<AppUser> GetUnconfirmedUserAsync(string email)
        {
            string userDataJsonString = await _cache.GetStringAsync(email);
            AppUser appUser = JsonSerializer.Deserialize<AppUser>(userDataJsonString);
            _cache.Remove(email);

            return appUser;
        }

        public async Task<IdentityResult> ConfirmUserAsync(AppUser appUser)
        {
            appUser.EmailConfirmed = true;
       
            //Should contain hashed password, so we dont need to pass password
            IdentityResult result = await _userManager.CreateAsync(appUser);
            IdentityResult roleResult = await _userManager.AddToRoleAsync(appUser, "User");

            return result;
        }

    }
}
