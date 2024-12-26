﻿using Azure;
using fightnight.Server.Abstracts;
using fightnight.Server.Data;
using fightnight.Server.Dtos;
using fightnight.Server.Dtos.Account;
using fightnight.Server.Enums;
using fightnight.Server.Extensions;
using fightnight.Server.Factories;
using fightnight.Server.Interfaces;
using fightnight.Server.Interfaces.IRepos;
using fightnight.Server.Interfaces.IServices;
using fightnight.Server.Models.Tables;
using fightnight.Server.Providers.EmailProviders;
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
        private readonly AppDBContext _context;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly IInviteService _inviteService;
        private readonly ITokenService _tokenService;
        private readonly IOAuthService _OAuthService;
        private readonly IEmailService _emailService;
        private readonly IAuthService _AuthService;


        public AccountController(
            UserManager<AppUser> userManager,
            OAuthProviderFactory providerFactory,
            AppDBContext context,
            SignInManager<AppUser> signInManager,

            IInviteService inviteService,
            ITokenService tokenService,
            IOAuthService OAuthService,
            IAuthService AuthService,
            IEmailService emailService)
        {
            _userManager = userManager;
            _providerFactory = providerFactory;
            _context = context;
            _signInManager = signInManager;

            _inviteService = inviteService;
            _tokenService = tokenService;
            _OAuthService = OAuthService;
            _AuthService = AuthService;
            _emailService = emailService;
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

                if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
                {
                    return BadRequest("Email Already Taken");
                }

                AppUser appUser = AppUserFactory.CreateAppUser(registerDto.Username, registerDto.Email);

                Invitation invite = await _inviteService.UpdateUserAsync(appUser, registerDto.inviteId, Response);

                await _AuthService.RegisterUserAsync(appUser, registerDto.Password);
                IActionResult result = Ok("User has been Registered");

                if (!appUser.EmailConfirmed)
                {
                    Email email = new RegisterConfirmEmail(registerDto.Email, _tokenService);
                    await _emailService.SendEmail(email);

                    result = Ok("Verification Email has been Sent");
                }

                await _inviteService.AddUserToEventAsync(invite);

                return result;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPatch("verify")]
        public async Task<IActionResult> VerifyEmail([FromBody] string token)
        {
            if (token == null)
            {
                return BadRequest("Missing Token");
            }

            IEnumerable<Claim> claims = _tokenService.DecodeToken(token);

            var exp = claims.GetClaimValue("exp");

            if (DateTime.Parse(exp) < DateTime.Now)
            {
                // Send new email
                return BadRequest("Token Expired");
            }
            
            string email = claims.GetClaimValue("email");

            AppUser appUser = await _userManager.FindByEmailAsync(email);

            if (appUser == null)
            {
                return BadRequest("User does not Exist.");
            }
            else if (appUser.EmailConfirmed)
            {
                return BadRequest("User Already Verified");
            }

            appUser.EmailConfirmed = true;
            var result = await _userManager.UpdateAsync(appUser);

            if (!result.Succeeded)
            {
                return BadRequest("There has been an Error Verifying User");
            }
            
            return Ok("Email has been Verified");
        }

        [HttpGet("ping")]
        public IActionResult pingAuth()
        {
            return Ok(AppUserFactory.ReturnAuthedUser(User));
        }
     

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                AppUser appUser = await _userManager.FindByEmailAsync(loginDto.Email);

                if (appUser == null)
                {
                    return Unauthorized("Email not found");
                }
                if (!appUser.EmailConfirmed)
                {
                    // Send Email
                    return Unauthorized("Confirm Your Email before logging in");
                }

                appUser = await _AuthService.LogUserInAsync(appUser, loginDto.Password, loginDto.rememberMe);

                Invitation invite = await _inviteService.UpdateUserAsync(appUser, loginDto.inviteId, Response);
                await _inviteService.AddUserToEventAsync(invite);

                return Ok(
                    AppUserFactory.ReturnAuthedUser(User)
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("https://localhost:5173/");
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

                AppUser appUser = await _userManager.FindByEmailAsync(user.UserEmail);

                if (appUser == null)
                {
                    appUser = AppUserFactory.CreateAppUser(user.UserName, user.UserEmail);
                    appUser.EmailConfirmed = true;
                    await _AuthService.RegisterUserAsync(appUser);
                }
                
                await _AuthService.LogUserInAsync(appUser);

                return Redirect("https://localhost:5173/home");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // ----------------------------------------------










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
            await _emailService.SendEmail(emailTemp);

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
