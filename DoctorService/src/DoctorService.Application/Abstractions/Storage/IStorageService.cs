namespace ProductService.Application.Abstractions.Storage
{
    public interface IStorageService
    {
        Task<UploadObjectResult> UploadAsync(Stream content, string objectKey, string contentType, Dictionary<string, string>? metadata = null, CancellationToken cancellationToken = default);
        Task DeleteAsync(string objectKey, CancellationToken cancellationToken = default);
        string? GetReadUrl(string objectKey);
    }
}
