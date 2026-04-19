namespace ProductService.Api.Contracts.Requests
{
    public sealed class UploadProductImageRequest
    {
        public IFormFile File { get; set; } = default!;
        public bool IsMain { get; set; }
        public int? SortOrder { get; set; }
    }
}
