using fightnight.Server.Data;
using fightnight.Server.Dtos;
using fightnight.Server.Dtos.Account;
using fightnight.Server.Enums;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Interfaces.IServices;
using fightnight.Server.Models.Tables;
using fightnight.Server.Services;
using MetInProximityBack.Factories;
using MetInProximityBack.Interfaces;
using MetInProximityBack.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

namespace fightnight.Server.Controllers
{
    [Route("api/account")]
    [ApiController]
    
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly OAuthProviderFactory _providerFactory;
        private readonly ITokenService _tokenService;
        private readonly IOAuthService _OAuthService;
        private readonly IInviteRepo _inviteRepo;
        private readonly IMemberRepo _memberRepo;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly AppDBContext _context;

        public AccountController(
            UserManager<AppUser> userManager,
            OAuthProviderFactory providerFactory,
            ITokenService tokenService,
            IOAuthService OAuthService,
            IInviteRepo inviteRepo,
            SignInManager<AppUser> signInManager, 
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService,
            AppDBContext context)
        {
            _userManager = userManager;
            _providerFactory = providerFactory;
            _tokenService = tokenService;
            _OAuthService = OAuthService;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _context = context;
            _inviteRepo = inviteRepo;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == registerDto.Email.ToLower());
                if (user != null){
                    return BadRequest("Email already Taken");
                }

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                };

                if (registerDto.inviteId != null)
                {
                    Invitation invite = await _inviteRepo.GetInvitationAsync(registerDto.inviteId);
                    if (invite != null)
                    {
                        Response.Redirect("https://localhost:5173/" + invite.eventId + "/team");

                        var AppUserEvent = new AppUserEvent
                        {
                            EventId = invite.eventId,
                            AppUserId = appUser.Id,
                            Role = EventRole.Moderator,
                        };
                        await _memberRepo.AddMemberToEventAsync(AppUserEvent);

                        appUser.EmailConfirmed = true;
                    }
                }
                
                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        if (!appUser.EmailConfirmed)
                        {
                            string token = Guid.NewGuid().ToString();
                            DateTime expiry = DateTime.Now.AddMinutes(5);

                            var existingToken = await _context.UserToken.FirstOrDefaultAsync(x => x.userId == appUser.Id);
                            if (existingToken != null)
                            {
                                return BadRequest("A confirmation email has already been sent");
                            }

                            var userToken = new UserToken
                            {
                                userId = appUser.Id,
                                //userEmail = appUser.Email,
                                token = token,
                                expiry = expiry,
                                //tokenType = TokenType.verify
                            };

                            var createdEntry = _context.UserToken.AddAsync(userToken);
                            await _context.SaveChangesAsync();

                            string emailVerifyLink = "https://localhost:5173/verify-email?token=" + token + "&email=" + registerDto.Email;

                            var email = new ConfirmEmailTemplate
                            {
                                SendingTo = registerDto.Email,
                                EmailBody = "<p>Click <a href=" + emailVerifyLink
                                + ">Here</a> to verify your email.</p>"
                            };
                            await _emailService.Send(email);

                            return Ok("A Confirmation Email has been sent.");
                        }

                        return Ok("Registered with invite, account made and email confirmed");
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPatch("verify")]
        public async Task<IActionResult> VerifyEmail([FromBody] string token, string email)
        {
            UserToken userToken = await _context.UserToken.FirstOrDefaultAsync(x=> x.token == token && x.userEmail == email && x.tokenType.Equals(TokenType.verify));
            if (userToken == null)
            {
                return BadRequest("Invalid Verification Token");
            }
            string userId = userToken.userId;

            AppUser user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            
            _context.UserToken.Remove(userToken);
            _context.SaveChanges();

            if (user == null){
                return BadRequest("User does not Exist.");
            }
            else if (user.EmailConfirmed){
                
                return BadRequest("User Already Verified");
            }
            else if (userToken.expiry < DateTime.Now){
                return BadRequest("Token has Expired");
            }

            user.EmailConfirmed = true;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) {
                return BadRequest("There has been an Error Updating");
            }
            return Ok("Email has been Verified");
        }

        [HttpGet("ping")]
        public IActionResult pingAuth()
        {
            bool isAuthed = User.Identity.IsAuthenticated;
            
            if (isAuthed)
            {
                // get user info
                return Ok(new NewUserDto
                {
                    userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    userName = User.Identity.Name, 
                    email = User.FindFirstValue(ClaimTypes.Email),
                    picture = "edit claims type to have pfps", //User.FindFirstValue(ClaimTypes.Picture),
                    isAuthed = isAuthed, 
                    role = User.FindFirstValue(ClaimTypes.Role)
                });
            }
            return Ok(new { isAuthed });
        }

        [HttpPost("logout")]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();

            Response.Redirect("https://localhost:5173/");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email.ToLower());
            if (user == null){
                return Unauthorized("Email not found");
            }
            if (!user.EmailConfirmed)
            {
                return Unauthorized("Confirm Your Email before logging in");
            }

            var SignInResult = await _signInManager.PasswordSignInAsync(user.UserName, loginDto.Password, loginDto.rememberMe, true);
            if (SignInResult.IsLockedOut){
                //Send Email to this user saying someone is trying to access your account
                return Unauthorized("Account is locked");
            }
            else if (!SignInResult.Succeeded){
                return Unauthorized("Invalid Credentials");
            }

            //if invite param exists,
            //1. find invite to confirm

            Response.Redirect("https://localhost:5173/home");

            if (loginDto.inviteId != null)
            {
                Invitation invite = await _inviteRepo.GetInvitationAsync(loginDto.inviteId);
                if (invite != null)
                {
                    var AppUserEvent = new AppUserEvent
                    {
                        EventId = invite.eventId,
                        AppUserId = user.Id,
                        Role = EventRole.Moderator,
                    };
                    await _memberRepo.AddMemberToEventAsync(AppUserEvent);

                    Response.Redirect("https://localhost:5173/" + invite.eventId + "/team");
                }
            }

            return Ok(
                new NewUserDto
                {
                    userId = user.Id,
                    userName = user.UserName,
                    email = user.Email,
                    picture = user.Picture,
                    isAuthed = true,
                    role = User.FindFirstValue(ClaimTypes.Role)
                }
            );
        }

        [HttpPost("oauth/{provider}")]
        public async Task<IActionResult> Authenticate(
            [FromQuery(Name = "code")] string code,
            [FromRoute] string provider
        )
        {
            try
            {
                IOAuthProvider OAuthProvider = _providerFactory.GetProvider(provider);

                OAuthTokenResponse tokens = await _OAuthService.GetOAuthTokens(OAuthProvider.TokenUrl, OAuthProvider.GetReqValues(code));

                IEnumerable<Claim> claims = _tokenService.DecodeToken(tokens.id_token);

                OAuthUserDto user = await OAuthProvider.MapResponseToUser(claims);

                if (user.IsEmailVerified != true)
                {
                    return BadRequest("Email not verified.");
                }

                AppUser appUser = await _userManager.FindByEmailAsync(user.UserEmail) ?? await CreateAppUser(user.UserName, user.UserEmail);

                await _signInManager.SignInAsync(appUser, isPersistent: true);

                return Ok(user.UserEmail + " " + user.UserName);

                // change when android front end done 
                //return Redirect("https://localhost:5173/home");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> SendForgotPasswordEmail([FromBody] string email)
        {
            //check if token exists with email

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null){
                return BadRequest("Email Not Found");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            DateTime expiry = DateTime.Now.AddMinutes(5);

            var userToken = new UserToken
            {
                userId = user.Id,
                token = token,
                expiry = expiry
                //tokenType = TokenType.changePassword
            };

            var createdEntry = await _context.UserToken.AddAsync(userToken);
            await _context.SaveChangesAsync();

            var emailTemp = new ConfirmEmailTemplate
            {
                SendingTo = email,
                EmailBody = "The code to reset your password is:" + token
            };
            await _emailService.Send(emailTemp);

            return Ok("Email has been sent");
        }
        /*
        [HttpPost("verify-token")]
        public async Task<IActionResult> VerifyToken([FromBody] string token)
        {
            UserToken userToken = await _context.UserToken.FirstOrDefaultAsync(x => x.token == token && x.type.Equals(TokenType.changePassword));
            if (userToken == null){
                return BadRequest("Invalid Verification Token");
            }
        }
        */

        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordResetVals prVals)
        {
            UserToken userToken = await _context.UserToken.FirstOrDefaultAsync(x => x.token == prVals.token && x.userEmail == prVals.email && x.tokenType.Equals(TokenType.changePassword));
            if (userToken == null)
            {
                return BadRequest("Invalid Verification Token");
            }
            else if (userToken.expiry < DateTime.Now)
            {
                return BadRequest("Token has Expired");
            }

            AppUser user = await _userManager.FindByEmailAsync(userToken.userEmail);

            var result = await _userManager.ResetPasswordAsync(user, prVals.token, prVals.newPassword);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to update password");
            }
            return Ok("Password has been changed");
        }
    }
}
