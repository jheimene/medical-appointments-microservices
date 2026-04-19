
namespace BuildingBlocks.Application.Common.Errors
{
    public sealed record AppError(
        string Code,
        string Message,
        ErrorType Type,
        Dictionary<string, object>? Metadata = null
        );
}
