using Microsoft.AspNetCore.Http.HttpResults;
using OrderService.Api.Constracts.Requests;
using OrderService.Api.Constracts.Responses;
using OrderService.Application.Abstractions.Services;
using OrderService.Infrastructure.Idempotency.Helpers;
using System.Text;

namespace OrderService.Api.Filters
{
    public sealed class IdempotencyEndpointFilter(
        IOrderIdempotencyService idEmpotencyService,
        ILogger<IdempotencyEndpointFilter> logger
        ) : IEndpointFilter
    {
        private readonly IOrderIdempotencyService _idempotencyService = idEmpotencyService;
        private readonly ILogger<IdempotencyEndpointFilter> _logger = logger;


        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var httpContext = context.HttpContext;
            var cancellationToken = httpContext.RequestAborted;

            if (!TryGetIdempotencyKey(httpContext, out var idempotencyKey))
            {
                return TypedResults.BadRequest(new ErrorResponse(
                    "IDEMPOTENCY_KEY_REQUIRED",
                    "El header Idempotency-Key es obligatorio."));
            }

            var request = context.Arguments
                .OfType<CreateOrderRequest>()
                .FirstOrDefault();

            if (request is null)
            {
                return TypedResults.BadRequest(new ErrorResponse(
                    "INVALID_REQUEST",
                    "No se pudo obtener el payload de la solicitud."));
            }

            var requestBody = IdempotencyJson.Serialize(request);

            var beginResult = await _idempotencyService.BeginAsync(idempotencyKey, requestBody, cancellationToken);

            if (!beginResult.CanExecute)
            {
                if (beginResult.HasCachedResponse)
                {
                    _logger.LogInformation("Idempotency HIT. IdempotencyKey={IdempotencyKey}", idempotencyKey);

                    return Results.Content(
                        beginResult.CachedResponseBody ?? "{}",
                        "application/json",
                        Encoding.UTF8,
                        beginResult.CachedStatusCode ?? StatusCodes.Status200OK);
                }

                _logger.LogWarning("Idempotency blocked request. IdempotencyKey={IdempotencyKey}, Reason={Reason}", idempotencyKey, beginResult.ErrorMessage);

                return TypedResults.Conflict(new ErrorResponse(
                    "IDEMPOTENCY_CONFLICT",
                    beginResult.ErrorMessage ?? "La solicitud ya fue procesada o está en proceso."));
            }

            object? endpointResult;

            try
            {
                endpointResult = await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando endpoint con idempotencia. IdempotencyKey={IdempotencyKey}", idempotencyKey);

                await _idempotencyService.ReleaseAsync(idempotencyKey, cancellationToken);

                throw;
            }


            switch (endpointResult)
            {
                case Created<CreateOrderResponse> createdResult:
                    {
                        var response = createdResult.Value;

                        if (response is not null)
                        {
                            var responseJson = IdempotencyJson.Serialize(response);

                            await _idempotencyService.CompleteAsync(
                                idempotencyKey,
                                requestBody,
                                StatusCodes.Status201Created,
                                responseJson,
                                response.OrderId.ToString(),
                                cancellationToken);

                            _logger.LogInformation("Idempotency completed. IdempotencyKey={IdempotencyKey}, OrderId={OrderId}", idempotencyKey, response.OrderId);
                        }

                        return endpointResult;
                    }

                case Ok<CreateOrderResponse> okResult:
                    {
                        var response = okResult.Value;

                        if (response is not null)
                        {
                            var responseJson = IdempotencyJson.Serialize(response);

                            await _idempotencyService.CompleteAsync(
                                idempotencyKey,
                                requestBody,
                                StatusCodes.Status200OK,
                                responseJson,
                                response.OrderId.ToString(),
                                cancellationToken);

                            _logger.LogInformation("Idempotency completed with OK. IdempotencyKey={IdempotencyKey}, OrderId={OrderId}", idempotencyKey, response.OrderId);
                        }

                        return endpointResult;
                    }

                case IStatusCodeHttpResult statusCodeResult
                    when statusCodeResult.StatusCode is >= 400:
                    {
                        _logger.LogWarning(
                            "Endpoint returned non-success status. IdempotencyKey={IdempotencyKey}, StatusCode={StatusCode}",
                            idempotencyKey,
                            statusCodeResult.StatusCode);

                        await _idempotencyService.ReleaseAsync(
                            idempotencyKey,
                            cancellationToken);

                        return endpointResult;
                    }

                default:
                    {
                        _logger.LogWarning(
                            "Unhandled endpoint result type for idempotency. IdempotencyKey={IdempotencyKey}, ResultType={ResultType}",
                            idempotencyKey,
                            endpointResult?.GetType().FullName ?? "null");

                        await _idempotencyService.ReleaseAsync(
                            idempotencyKey,
                            cancellationToken);

                        return endpointResult;
                    }
            }

        }


        private static bool TryGetIdempotencyKey(HttpContext httpContext, out string idempotencyKey)
        {
            idempotencyKey = string.Empty;

            if (!httpContext.Request.Headers.TryGetValue("Idempotency-Key", out var idemHeader))
                return false;

            idempotencyKey = idemHeader.ToString().Trim();
            return !string.IsNullOrWhiteSpace(idempotencyKey);
        }
    }
}
