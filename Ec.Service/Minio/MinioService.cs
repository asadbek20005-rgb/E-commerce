using Ec.Common.Models.Minio;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Ec.Service.Minio;

public class MinioService
{
    private readonly MinioClient _minioClient;
    private readonly string _bucketName;

    public MinioService(IOptions<MinIOSettings> minioSettings)
    {
        var settings = minioSettings.Value;

        _minioClient = (MinioClient)new MinioClient()
            .WithEndpoint(settings.Endpoint)
            .WithCredentials(settings.AccessKey, settings.SecretKey);

        _bucketName = settings.BucketName;
    }

    public async Task EnsureBucketExistsAsync()
    {
        var found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
        if (!found)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
        }
    }


    public async Task UploadFileAsync(string objectName, Stream data, long size, string contentType)
    {
        try
        {
            await EnsureBucketExistsAsync();
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithStreamData(data)
                .WithObjectSize(size)
                .WithContentType(contentType));

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
