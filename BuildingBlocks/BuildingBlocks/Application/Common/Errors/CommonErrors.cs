namespace BuildingBlocks.Application.Common.Errors
{
    public static class CommonErrors
    {
        public static AppError Validation(string code, string message, Dictionary<string, object>? metadata = null)
        => new(code, message, ErrorType.Validation, metadata);

        public static AppError NotFound(string code, string message, Dictionary<string, object>? metadata = null)
            => new(code, message, ErrorType.NotFound, metadata);

        public static AppError Conflict(string code, string message, Dictionary<string, object>? metadata = null)
            => new(code, message, ErrorType.Conflict, metadata);

        public static AppError Unauthorized(string message = "No autorizado.")
            => new("COMMON.UNAUTHORIZED", message, ErrorType.Unauthorized);

        public static AppError Forbidden(string message = "Acceso denegado.")
            => new("COMMON.FORBIDDEN", message, ErrorType.Forbidden);

        public static AppError External(string code, string message, Dictionary<string, object>? metadata = null)
            => new(code, message, ErrorType.External, metadata);

        public static AppError Unexpected(string message = "Ocurrió un error interno.")
            => new("COMMON.UNEXPECTED", message, ErrorType.Unexpected);
    }
}
