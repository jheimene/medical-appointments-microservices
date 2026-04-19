using Security.ApiGateway.Yarp.Abstractions;
using Security.ApiGateway.Yarp.Contracts;

namespace Security.ApiGateway.Yarp.Configuration
{
    public class FileGatewayRuntimeApplier : IGatewayRuntimeApplier
    {
        public Task ApplyAsync(GatewayConfigSnapshot snapshot, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
