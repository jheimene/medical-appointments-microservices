namespace ProductService.Application.Common.Options
{
    public sealed class ImageRulesOptions
    {
        public int MaxImagesPerProduct { get; set; } = 10;
        public long MaxSizeBytes { get; set; } = 5 * 1024 * 1024;
        public string[] AllowedContentTypes { get; set; } = [
            "image/jpeg",
            "image/png",
            "image/webp"
        ];
    }
}
