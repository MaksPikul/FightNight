using fightnight.Server.Dtos.Account;
using fightnight.Server.Models.Tables;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fightnight.Server.Factories
{
    public class AppUserFactory
    {
        // Could make UserDto class, then extend wether login Dto or Register Dto
        public static AppUser CreateAppUser(string userName, string email, string picture = null)
        {
            return new AppUser
            {
                UserName = userName,
                Email = email,
                Picture = picture,
                EmailConfirmed = false
            };
        }

        public static AuthedUserResDto CreateAuthedUser(ClaimsPrincipal User) 
        {
            bool isAuthed = User.Identity.IsAuthenticated;

            if (!isAuthed)
            {
                return new AuthedUserResDto();
            }
            else
            {
                return new AuthedUserResDto
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    UserName = User.Identity.Name,
                    Email = User.FindFirstValue(ClaimTypes.Email),
                    Picture = User.FindFirstValue("Picture"),
                    IsAuthed = true,
                    Role = User.FindFirstValue(ClaimTypes.Role)
                };
            }
        }

    }
}
