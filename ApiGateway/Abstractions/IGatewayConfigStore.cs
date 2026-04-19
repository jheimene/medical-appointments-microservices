using Security.ApiGateway.Yarp.Contracts;

namespace Security.ApiGateway.Yarp.Abstractions
{
    public interface IGatewayConfigStore
    {
        Task<GatewayConfigSnapshot> GetAsync(CancellationToken cancellationToken);
        Task SaveAsync(GatewayConfigSnapshot snapshot, CancellationToken cancellationToken);
    }
}
