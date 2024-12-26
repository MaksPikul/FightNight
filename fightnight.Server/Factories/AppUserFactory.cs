using fightnight.Server.Dtos.Account;
using fightnight.Server.Models.Tables;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace fightnight.Server.Factories
{
    public class AppUserFactory
    {
        // Could make UserDto class, then extend wether login Dto or Register Dto
        public static AppUser CreateAppUser(string userName, string email)
        {
            return new AppUser
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = false
            };
        }

        public static AuthedUserDto ReturnAuthedUser(ClaimsPrincipal User) 
        {
            bool isAuthed = User.Identity.IsAuthenticated;

            if (!isAuthed)
            {
                return new AuthedUserDto();
            }
            else
            {
                return new AuthedUserDto
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    UserName = User.Identity.Name,
                    Email = User.FindFirstValue(ClaimTypes.Email),
                    Picture = "edit claims type to have pfps", //User.FindFirstValue(ClaimTypes.Picture),
                    IsAuthed = true,
                    Role = User.FindFirstValue(ClaimTypes.Role)
                };
            }
        }

    }
}
