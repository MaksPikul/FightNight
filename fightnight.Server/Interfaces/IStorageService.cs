using Amazon.S3.Model;
using fightnight.Server.Dtos.S3;
using fightnight.Server.Models;

namespace fightnight.Server.Interfaces
{
    public interface IStorageService
    {
        Task<S3ResponseDto> UploadFileAsync(S3Obj s3Obj, string key, string secret);
    }
}
