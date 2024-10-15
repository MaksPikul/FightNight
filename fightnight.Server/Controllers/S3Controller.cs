using fightnight.Server.Data;
using fightnight.Server.Extensions;
using fightnight.Server.Interfaces.IServices;
using fightnight.Server.Models.Tables;
using fightnight.Server.Models.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace fightnight.Server.Controllers
{
    [ApiController]
    [Route("api/s3")]
    public class S3Controller : Controller
    {
        private readonly IStorageService _storageService;
        private readonly IConfiguration _config;
        private readonly AppDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        public S3Controller(
            IStorageService storageService,
            IConfiguration configuration,
            AppDBContext context,
            UserManager<AppUser> userManager) 
        {
            _storageService = storageService;
            _config = configuration;
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("upload-pfp")]
        [Authorize]
        public async Task<IActionResult> UploadPFP(IFormFile file)
        {
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync( memoryStream );

            var fileExt = Path.GetExtension( file.Name );
            var objName = $"{Guid.NewGuid()}.{fileExt}";

            var s3Obj = new S3Obj
            {
                BucketName = "fightnightbucket",
                InputStream = memoryStream,
                Name = objName
            };

            var result = await _storageService.UploadFileAsync(
                s3Obj,
                _config["AWSConfig:AccessKey"],
                _config["AWSConfig:SecretKey"]
                );

            Console.WriteLine( objName );

            var email = User.GetEmail();
            var user = await _userManager.FindByEmailAsync(email);

            user.ProfilePicture = objName;
            await _userManager.UpdateAsync(user);

            // pfp has been updated
            return Ok( result );
        }
    }
}
