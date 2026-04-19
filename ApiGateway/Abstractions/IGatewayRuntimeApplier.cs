using Security.ApiGateway.Yarp.Contracts;

namespace Security.ApiGateway.Yarp.Abstractions
{
    public interface IGatewayRuntimeApplier
    {
        Task ApplyAsync(GatewayConfigSnapshot snapshot, CancellationToken cancellationToken);
    }
}
