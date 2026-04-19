
using MediatR;
using Microsoft.Extensions.Logging;

namespace OrderService.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellation)
        {
            var name = typeof(TRequest).Name;
            logger.LogInformation("Handling {RequestName} {Request}", name, request);
            var response = await next();
            logger.LogInformation("Handled {Request}", name);
            return response;
        }
    }
}
