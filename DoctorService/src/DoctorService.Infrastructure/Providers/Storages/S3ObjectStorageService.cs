using Amazon.S3;
using Amazon.S3.Model;
using ProductService.Application.Abstractions.Storage;
using ProductService.Infrastructure.Configuration;

namespace ProductService.Infrastructure.Providers.Storages
{
    public class S3ObjectStorageService : IStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly S3StorageOptions _options;

        public S3ObjectStorageService(IAmazonS3 s3Client, S3StorageOptions options)
        {
            _s3Client = s3Client;
            _options = options;
        }

        public async Task<UploadObjectResult> UploadAsync(
            Stream content,
            string objectKey,
            string contentType,
            Dictionary<string, string>? metadata = null,
            CancellationToken cancellationToken = default)
        {
            if (content.CanSeek)
                content.Position = 0;

            var request = new PutObjectRequest
            {
                BucketName = _options.BucketName,
                Key = objectKey,
                InputStream = content,
                ContentType = contentType
            };

            if (metadata is not null)
            {
                foreach (var item in metadata)
                    request.Metadata[item.Key] = item.Value;
            }

            await _s3Client.PutObjectAsync(request, cancellationToken);

            return new UploadObjectResult(objectKey, GetReadUrl(objectKey));
        }

        public async Task DeleteAsync(string objectKey, CancellationToken cancellationToken = default)
        {
            await _s3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _options.BucketName,
                Key = objectKey
            }, cancellationToken);
        }

        public string? GetReadUrl(string objectKey)
        {
            if (!string.IsNullOrWhiteSpace(_options.PublicBaseUrl))
                return $"{_options.PublicBaseUrl!.TrimEnd('/')}/{EncodePath(objectKey)}";

            return $"https://{_options.BucketName}.s3.amazonaws.com/{EncodePath(objectKey)}";
        }

        private static string EncodePath(string path)
            => string.Join("/", path.Split('/', StringSplitOptions.RemoveEmptyEntries)
                .Select(Uri.EscapeDataString));
    }
}
