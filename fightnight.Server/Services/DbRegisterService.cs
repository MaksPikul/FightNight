using fightnight.Server.Interfaces;
using fightnight.Server.Interfaces.IServices;
using fightnight.Server.Models.Tables;
using Microsoft.AspNetCore.Identity;

namespace fightnight.Server.Services
{
    // Primary Constructor, Learnt about this c# feature, looks very interesting
    public class DbRegisterService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IEmailService emailService,
        IInviteService inviteService,
        ITokenService tokenService,
        IRegisterService registerService
    ) 
        : 
    AuthService(userManager, signInManager, emailService, inviteService, tokenService, registerService), IRegisterService
    {
        public async Task<AppUser> AddUnconfirmedUserAsync(AppUser appUser, string password = null)
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
                throw new Exception("Internal Error Adding Role to User, ERRORS:" + roleResult.Errors);
            }

            return appUser;
        }

        public async Task<AppUser> GetUnconfirmedUserAsync(string email)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(email);
            return appUser;
        }

        public async Task<IdentityResult> ConfirmUserAsync(AppUser appUser)
        {
            appUser.EmailConfirmed = true;
            IdentityResult result = await _userManager.UpdateAsync(appUser);

            return result;
        }

    }
}
