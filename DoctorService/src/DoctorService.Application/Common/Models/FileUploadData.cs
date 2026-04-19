namespace ProductService.Application.Common.Models
{
    public sealed record FileUploadData(
        string FileName,
        string ContentType,
        long Length,
        Stream Content
    );
}
