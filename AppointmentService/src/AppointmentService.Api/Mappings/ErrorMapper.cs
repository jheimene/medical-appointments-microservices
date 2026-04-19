using BuildingBlocks.Application.Common.Errors;

namespace OrderService.Api.Mappings
{
    public static class ErrorMapper
    {
        public static IResult ToHttpResult(AppError error)
        {
            var payload = new
            {
                code = error.Code,
                message = error.Message,
                type = error.Type.ToString(),
                metadata = error.Metadata
            };

            return error.Type switch
            {
                ErrorType.Validation => Results.BadRequest(payload),
                ErrorType.NotFound => Results.NotFound(payload),
                ErrorType.Conflict => Results.Conflict(payload),
                ErrorType.Unauthorized => Results.Unauthorized(),
                ErrorType.Forbidden => Results.Json(payload, statusCode: 403),
                ErrorType.BusinessRule => Results.BadRequest(payload),
                ErrorType.External => Results.Json(payload, statusCode: 502),
                _ => Results.Json(payload, statusCode: 500)
            };
        }
    }
}
