namespace ProductService.Infrastructure.Configuration
{
    public sealed class StorageOptions
    {
        public const string SectionName = "Storage";
        public StorageProvider Provider { get; set; } = StorageProvider.S3;
        public S3StorageOptions S3 { get; set; } = new();
        public AzureBlobStorageOptions AzureBlob { get; set; } = new();
    }

    public enum StorageProvider
    {
        S3 = 1,
        AzureBlob = 2
    }
}
