
namespace ProductService.Application.Abstractions.Storage
{
    public sealed record UploadObjectResult(
        string ObjectKey,
        string? ReadUrl
    );
}
