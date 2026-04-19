using BuildingBlocks.Application.Common.Errors;

namespace BuildingBlocks.Application.Common.Exceptions
{
    public sealed class ApplicationException(AppError error) : Exception(error.Message)
    {
        public AppError Error { get; } = error;   
    }
}
