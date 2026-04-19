using BuildingBlocks.Api.ErrorHandling;
using BuildingBlocks.Application.Common.Errors;
using FluentValidation;
using System.Net;

namespace OrderService.Api.Middleware
{
    public sealed class ExceptionHandlingMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
            {
                _logger.LogWarning("La solicitud fue cancelada por el cliente. TraceId: {TraceId}", context.TraceIdentifier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Se produjo una excepción no controlada. TraceId: {TraceId}", context.TraceIdentifier);

                var (statusCode, response) = MapException(ex, context);

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(response);
            }
        }

        private (int statusCode, ErrorResponse response) MapException(Exception exception, HttpContext context)
        {
            AppError error;

            switch (exception)
            {
                case BuildingBlocks.Application.Common.Exceptions.ApplicationException appException:
                    error = appException.Error;
                    break;

                case ValidationException validationException:
                    error = BuildValidationError(validationException);
                    break;

                case UnauthorizedAccessException:
                    error = CommonErrors.Unauthorized();
                    break;

                case KeyNotFoundException:
                    error = CommonErrors.NotFound("COMMON.NOT_FOUND", "No se encontró el recurso solicitado.");
                    break;

                case TimeoutException:
                    error = CommonErrors.External(
                        "COMMON.TIMEOUT",
                        "La operación excedió el tiempo permitido.");
                    break;

                case HttpRequestException:
                    error = CommonErrors.External(
                        "COMMON.EXTERNAL_ERROR",
                        "Ocurrió un error al comunicarse con un servicio externo.");
                    break;

                default:
                    error = CommonErrors.Unexpected(
                        _environment.IsDevelopment()
                            ? exception.Message
                            : "Ocurrió un error interno del servidor.");
                    break;
            }

            var statusCode = MapStatusCode(error.Type);

            var response = new ErrorResponse(
                Success: false,
                Error: new ErrorDetail(
                    Code: error.Code,
                    Message: error.Message,
                    Type: error.Type.ToString(),
                    Metadata: error.Metadata,
                    TraceId: context.TraceIdentifier));

            return (statusCode, response);
        }

        private static int MapStatusCode(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => (int)HttpStatusCode.BadRequest,
                ErrorType.NotFound => (int)HttpStatusCode.NotFound,
                ErrorType.Conflict => (int)HttpStatusCode.Conflict,
                ErrorType.Unauthorized => (int)HttpStatusCode.Unauthorized,
                ErrorType.Forbidden => (int)HttpStatusCode.Forbidden,
                ErrorType.BusinessRule => (int)HttpStatusCode.BadRequest,
                ErrorType.External => (int)HttpStatusCode.BadGateway,
                ErrorType.Unexpected => (int)HttpStatusCode.InternalServerError,
                _ => (int)HttpStatusCode.InternalServerError
            };
        }

        private static AppError BuildValidationError(ValidationException validationException)
        {
            var errors = validationException.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.ErrorMessage).Distinct().ToArray());

            return new AppError(
                Code: "COMMON.VALIDATION_ERROR",
                Message: "Uno o más errores de validación ocurrieron.",
                Type: ErrorType.Validation,
                Metadata: new Dictionary<string, object>
                {
                    ["errors"] = errors
                });
        }
    }
}
