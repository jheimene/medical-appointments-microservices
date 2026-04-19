namespace ProductService.Application.Products.Dtos
{
    public sealed record ProductImageDto(
        Guid ProductImageId,
        Guid ProductId,
        string ObjectKey,
        string OriginalFileName,
        string ContentType,
        long SizeBytes,
        bool IsMain,
        int SortOrder,
        string Status,
        string? ReadUrl,
        DateTime CreateAt
    );
}
