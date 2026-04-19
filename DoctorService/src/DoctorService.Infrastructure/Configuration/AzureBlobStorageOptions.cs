namespace ProductService.Infrastructure.Configuration
{
    public sealed class AzureBlobStorageOptions
    {
        public string AccountName { get; set; } = string.Empty;
        public string ContainerName { get; set; } = string.Empty;
        public bool UseManagedIdentity { get; set; } = true;
        public string? AccountKey { get; set; }
        public string? PublicBaseUrl { get; set; }
    }
}
