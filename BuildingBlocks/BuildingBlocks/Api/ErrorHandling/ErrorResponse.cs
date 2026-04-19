
namespace BuildingBlocks.Api.ErrorHandling
{
    public sealed record ErrorResponse(
        bool Success,
        ErrorDetail Error
    );  
}
