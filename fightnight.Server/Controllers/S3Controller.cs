using fightnight.Server.Data;
using fightnight.Server.Interfaces;
using fightnight.Server.Models;
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
        public S3Controller(
            IStorageService storageService,
            IConfiguration configuration,
            AppDBContext context) 
        {
            _storageService = storageService;
            _config = configuration;
            _context = context;
        }

        [HttpPost("upload-file")]
        public async Task<IActionResult> UploadFile(IFormFile file)
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

            // add url of saved file into event Db

            return Ok( result );
        }
    }
}
