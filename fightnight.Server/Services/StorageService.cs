using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using fightnight.Server.Dtos.S3;
using fightnight.Server.Interfaces.IServices;
using fightnight.Server.Models.Types;
using Microsoft.AspNetCore.Http.HttpResults;

namespace fightnight.Server.Services
{
    public class StorageService : IStorageService
    {
        //private readonly IAmazonS3 _s3Client;
        public StorageService(/*IAmazonS3 s3Client*/) { 
            //_s3Client = s3Client;
        }
        public async Task<S3ResponseDto> UploadFileAsync(S3Obj s3Obj, string accessKey, string secret)
        {
            var credentials = new BasicAWSCredentials(accessKey, secret);
            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUWest2
            };

            var response = new S3ResponseDto();

            try
            {
                var uploadRequest = new TransferUtilityUploadRequest()
                { 
                    InputStream = s3Obj.InputStream,
                    Key = s3Obj.Name,
                    BucketName = s3Obj.BucketName,
                    CannedACL = S3CannedACL.PublicRead
                };

                var client = new AmazonS3Client(credentials, config);
                var transferUtility = new TransferUtility(client);

                await transferUtility.UploadAsync(uploadRequest);

                response.StatusCode = 200;
                response.Message = $"{s3Obj.Name} has been uploaded successfully";
            }
            catch (AmazonS3Exception ex)
            {
                response.StatusCode = (int)ex.StatusCode;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
