using fightnight.Server.Data;
using fightnight.Server.Dtos;
using fightnight.Server.Dtos.Account;
using fightnight.Server.Enums;
using fightnight.Server.Interfaces.IServices;
using fightnight.Server.Models.Tables;
using fightnight.Server.Services;
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
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IGoogleTokenService _googleTokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly AppDBContext _context;

        public AccountController(
            UserManager<AppUser> userManager, 
            ITokenService tokenService, 
            SignInManager<AppUser> signInManager, 
            IGoogleTokenService googleTokenService,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService,
            AppDBContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;   
            _signInManager = signInManager;
            _googleTokenService = googleTokenService;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _context = context;
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
                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        //this will need to be changed to use redis or sum

                        /*
                         Random random = new Random();
                        int randomNumber = random.Next(100000, 1000000); // Generates a number between 100000 and 999999
                        string token = randomNumber.ToString();
                         */

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

                        var email = new ConfirmEmailTemplate {
                            SendingTo = registerDto.Email,
                            EmailBody = "<p>Click <a href=" + emailVerifyLink 
                            + ">Here</a> to verify your email.</p>"
                        };
                        await _emailService.Send(email);

                        return Ok("A Confirmation Email has been sent.");
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
            var isAuthed = User.Identity.IsAuthenticated;
            
            if (isAuthed)
            {
                // get user info
                return Ok(new {
                    userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    userName = User.Identity.Name, 
                    email = User.FindFirstValue(ClaimTypes.Email),
                    //picture
                    isAuthed, 
                    role = User.FindFirstValue(ClaimTypes.Role)
                });
            }
            return Ok(new { isAuthed });
        }

        [HttpPost("logout")]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();

            //return Ok("Signed out");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email.ToLower());
            if (user == null){
                return Unauthorized("Email not found");
            }
            else if (!user.EmailConfirmed){
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

            return Ok(
                new NewUserDto
                {
                    userId = user.Id,
                    userName = user.UserName,
                    email = user.Email,
                    // picture
                    isAuthed = true,
                    role = User.FindFirstValue(ClaimTypes.Role)
                }
            );
        }

        [HttpGet("oauth/google")]
        public async Task<IActionResult> OAuthGoogle([FromQuery(Name = "code")] string code)
        {
            var tokens = await _googleTokenService.getGoogleOAuthTokens(code);
            var googleUser = _tokenService.DecodeToken(tokens.id_token);

            var userVerified = googleUser.FirstOrDefault(c => c.Type == "email_verified").Value;
            if (userVerified == "false") 
            {
                return BadRequest("Email not verified");
            }
            var userPicture = googleUser.FirstOrDefault(c => c.Type == "picture").Value;
            var userEmail = googleUser.FirstOrDefault(c => c.Type == "email").Value;
            var userName = googleUser.FirstOrDefault(c => c.Type == "name").Value;

            var appUser = await _userManager.FindByEmailAsync(userEmail);

            if (appUser == null)
            {
                appUser = new AppUser
                {
                    UserName = userName,
                    Email = userEmail,
                    //Picture = userPicture
                };
                var createdUser = await _userManager.CreateAsync(appUser);
                var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            }

            await _signInManager.SignInAsync(appUser, true);

            return Redirect("https://localhost:5173/home");
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
