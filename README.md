<h1>Fight Night, Combat Sport Event Manager for Clubs and Societies </h1>
<h4> Project in which I hope to learn about the ASP.Net Core ecosystem for backend development, as well as coding principles and design patterns</h4>
<br>

<h2> Tech Stack </h2>
<h3>Front-End <h5>(Acceptable BareBones UI, Main concerns are the back end functionalities)</h5> </h3>
<ul>
  <li> Typescript </li>
  <li> React </li>
  <li> Tailwind CSS </li>
  <li> Shadcn UI Components </li>
</ul>

<h3>Back-End</h3>
<ul>
  <li> C# </li>
  <li> ASP.NET Core 8.0 </li>
</ul>

Documentation and code samples coming Up Soon, will talk about <br>
- design patterns used and SOLID principles followed
- Account Controller (credential &OAuth login) (theres actually alot of cool stuff in this controller, in my humble opinion)
- Message Controller and Chat Hub (Caching Connected Users, seperating concerns between contollers and hubs)
- Use of Entity Core Framework, some cool queries :D


OAuthenticate method in AccountController class <br>
After Client sends a request to OAuth provider (google, microsoft, github, ...), the provider authentication server returns an access code and a string to denote where the request is coming from to handle it appropriately to the redirect URL provided, which is this API gateway (https://{domain}/api/account/oauth/{provider}?code=...)<br>

```C#
[HttpPost("oauth/{provider}")]
public async Task<IActionResult> OAuthenticate(
    [FromQuery(Name = "code")] string code,
    [FromRoute] string provider
)
{
    try
    {
        // Factory which returns a OAuthProvider class, 
        // this class contains methods and variables required to complete oauth 
        // implemented strategy/state pattern to change method behaviour during run time

        // To add more providers on the back end, All a developer would need to do would be to create another class which implements the IOAuthProvider interface, and fill in the required fields and methods
        // This follows the Open/Closed SOLID principle where a class is open for extension, but closed for modification
        IOAuthProvider OAuthProvider = _providerFactory.GetProvider(provider);

        // OAuth service using access code is now able to pass authentication when making requests to provider resource server (resource being user information)
        OAuthTokenResponse tokens = await _OAuthService.GetOAuthTokens(OAuthProvider.TokenUrl, OAuthProvider.GetReqValues(code));

        // Provider resource server returns a json of values, the Id token holding information about the user that we can use to authenticate them to our application
        // the id_token is a JWT which can be simply decoded into a enumerable object of claims
        IEnumerable<Claim> claims = _tokenService.DecodeToken(tokens.id_token);

        OAuthUserDto user = await OAuthProvider.MapResponseToUser(claims);

        if (user.IsEmailVerified != true)
        {
            return BadRequest("Email not verified.");
        }

        AppUser appUser = await _userManager.FindByEmailAsync(user.UserEmail);

        if (appUser == null)
        {
            appUser = AppUserFactory.CreateAppUser(user.UserName, user.UserEmail, user.Picture);
            await _AuthService.AddConfirmedUserAsync(appUser);
        }
        
        await _AuthService.LogUserInAsync(appUser, User);

        return Redirect("https://localhost:5173/home");
    }
    catch (ArgumentException ex)
    {
        return BadRequest(ex.Message);
    }
}
```
