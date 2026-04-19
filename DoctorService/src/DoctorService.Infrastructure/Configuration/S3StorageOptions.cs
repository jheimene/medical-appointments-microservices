namespace ProductService.Infrastructure.Configuration
{
    public sealed class S3StorageOptions
    {
        public string BucketName { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string? PublicBaseUrl { get; set; }
    }
}
